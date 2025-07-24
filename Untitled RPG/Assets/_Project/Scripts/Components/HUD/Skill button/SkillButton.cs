using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private static Skill lastSelectedSkill;

    private static NTask holdingRoutine;

    [TitleGroup("Settings")]
    [SerializeField]
    private Color32 interactableColor;

    [TitleGroup("Settings")]
    [SerializeField]
    private Color32 notInteractableColor;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text skillName;

    [TitleGroup("References")]
    [SerializeField]
    private Image background;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    [TitleGroup("References")]
    [SerializeField]
    private Image cooldownImage;

    [TitleGroup("References")]
    [SerializeField]
    private Image enabledFeedbackBorder;

    [TitleGroup("References")]
    [SerializeField]
    private Image holdIcon;

    [field: TitleGroup("Debug")]
    [field: ReadOnly]
    [field: SerializeField]
    public Skill EquippedSkill { get; private set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private bool canBePressed = true;

    private NTask cooldownRoutine;
    private IGameResource usingResource;

    private void Awake()
    {
        eventTrigger = GetComponent<EventTrigger>();
        GameEvents.OnBattleFinishingdEntryPoint += _ =>
        {
            cooldownRoutine?.Stop();
            cooldownRoutine = null;
        };
    }

    public void Setup (SetupSettings settings)
    {
        if (settings.skill is DoNothingSkill or null)
        {
            FillEmptySkill();
            return;
        }

        EquippedSkill = settings.skill;

        EquippedSkill.IsOnCooldown = false;
        SetupUsedResource(settings);
        TurnController.OnRanAction += RanActionListener;
        SetupHUD();

        SetupButtonInteraction(settings);
    }

    private void SetupButtonInteraction (SetupSettings settings)
    {
        var clickEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        clickEntry.callback.AddListener(_ =>
        {
            if (!canBePressed)
                return;

            if (lastSelectedSkill == null
                || EquippedSkill.Name != lastSelectedSkill.Name)
            {
                lastSelectedSkill = EquippedSkill;
                GameEvents.onChangedSkill?.Invoke(lastSelectedSkill);
            }

            var context = new OnSkillButtonPressedContext { Skill = EquippedSkill };
            settings.onPressedCallback?.Invoke(context);

            GameEvents.onSkillButtonPressed?.Invoke(context);

            if (EquippedSkill is not IHoldableSkill)
                return;

            // Trying to press more than one button
            if (holdingRoutine != null)
                return;

            if (ServiceLocator.BattleContext.Player.IsOnFatigue)
                return;

            if (EquippedSkill is ILevelCharge levelCharge)
            {
                levelCharge.OnFullCharged += () =>
                {
                    StartCoroutine(EquippedSkill.HoldFinish(new HoldFinishSettings
                    {
                        shouldHideBar = false,
                        shouldResetPoints = false
                    }));
                    holdingRoutine?.Stop();
                    holdingRoutine = null;

                    Blackboard.CurrentChargingSkill = null;
                };
            }

            holdingRoutine = new NTask(HoldingRoutine());
        });

        var holdEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        holdEntry.callback.AddListener(_ =>
        {
            StartCoroutine(EquippedSkill.HoldFinish(new HoldFinishSettings
            {
                shouldHideBar = false,
                shouldResetPoints = false
            }));
            CancelHold();
        });

        var moveOutsideEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        moveOutsideEntry.callback.AddListener(_ =>
        {
            StartCoroutine(EquippedSkill.HoldFinish(new HoldFinishSettings
            {
                shouldHideBar = false,
                shouldResetPoints = false
            }));
            CancelHold();
        });

        eventTrigger.triggers.Clear();
        eventTrigger.triggers.Add(clickEntry);
        eventTrigger.triggers.Add(holdEntry);
        eventTrigger.triggers.Add(moveOutsideEntry);
    }

    private IEnumerator HoldingRoutine()
    {
        if (!EquippedSkill.IsHoldableSkill)
            yield break;

        yield return new WaitForSeconds(.15f);
        yield return EquippedSkill.HoldBegin();
        Blackboard.CurrentChargingSkill = EquippedSkill;
    }

    private void CancelHold()
    {
        holdingRoutine?.Stop();
        holdingRoutine = null;

        Blackboard.CurrentChargingSkill = null;
    }

    private void SetupHUD()
    {
        cooldownImage.enabled = false;
        if (EquippedSkill != null)
        {
            EnableInteraction();
            holdIcon.enabled = EquippedSkill.IsHoldableSkill;
        }
        else
        {
            DisableInteraction();
            holdIcon.enabled = false;
        }

        skillName.text = EquippedSkill != null ? EquippedSkill.Name : "Empty";
    }

    private void SetupUsedResource (SetupSettings settings)
    {
        usingResource = EquippedSkill.SyncedResource = settings.usedResource;
        usingResource.OnUpdatedCurrent += (_, newValue) =>
        {
            if (cooldownRoutine != null)
                return;

            if (newValue >= EquippedSkill.ResourceCost)
                EnableInteraction();
            else
                DisableInteraction();
        };
    }

    private IEnumerator RanActionListener (BattleActionContext actionContext)
    {
        if (actionContext.skill != EquippedSkill)
            yield break;

        EnterCooldown(new CooldownSettings { duration = actionContext.skill.Cooldown });
    }

    public void EnterCooldown (CooldownSettings settings)
    {
        if (EquippedSkill.Cooldown == 0)
            return;

        cooldownRoutine = new NTask(Cooldown());

        IEnumerator Cooldown()
        {
            var elapsedSeconds = 0f;
            BattleManager.battleTickEntryPoint += MyCounter;

            cooldownImage.fillAmount = 1f;
            cooldownImage.enabled = true;
            DisableInteraction();

            EquippedSkill.IsOnCooldown = true;
            yield return new WaitUntil(() => elapsedSeconds >= settings.duration);
            EquippedSkill.IsOnCooldown = false;

            if (usingResource.Current >= EquippedSkill.ResourceCost)
                EnableInteraction();
            else
                DisableInteraction();

            cooldownImage.enabled = false;
            cooldownRoutine = null;

            IEnumerator MyCounter (BattleManager.TickContext ctx)
            {
                elapsedSeconds = Mathf.Min(elapsedSeconds + ctx.deltaTime, settings.duration);
                cooldownImage.fillAmount = 1f - elapsedSeconds / settings.duration;
                yield break;
            }
        }
    }

    private void FillEmptySkill()
    {
        EquippedSkill = null;
        DisableInteraction();
        skillName.text = string.Empty;

        eventTrigger.triggers.Clear();
    }

    public void SetSkillSelected (bool isSelected) => enabledFeedbackBorder.enabled = isSelected;

    private void EnableInteraction()
    {
        canBePressed = true;
        background.color = interactableColor;
    }

    private void DisableInteraction()
    {
        canBePressed = false;
        background.color = notInteractableColor;
    }

    public class CooldownSettings
    {
        public float duration;
    }

    public class SetupSettings
    {
        public Action<OnSkillButtonPressedContext> onPressedCallback;
        public Skill skill;
        public IGameResource usedResource;
    }
}