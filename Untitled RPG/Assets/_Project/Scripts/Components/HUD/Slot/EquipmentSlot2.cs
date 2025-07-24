using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public partial class EquipmentSlot2 : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    [TitleGroup("References")]
    [SerializeField]
    private Image equipmentIcon;

    [TitleGroup("References")]
    [SerializeField]
    private EquipmentSlotSkill defaultSkill;

    [TitleGroup("References")]
    [SerializeField]
    private EquipmentSlotSkill skillASlot;

    [TitleGroup("References")]
    [SerializeField]
    private EquipmentSlotSkill skillBSlot;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private EquipmentData currentEquipment;

    private NTask holdingRoutine;

    public IEnumerator Setup (Settings settings)
    {
        yield return SetupClickAndHoldInteraction();
        UpdateEquipment(settings.equipmentData);
    }

    private void UpdateEquipment (EquipmentData equipmentData)
    {
        currentEquipment = equipmentData;

        equipmentIcon.sprite = currentEquipment.Icon;

        defaultSkill.Setup(currentEquipment.DefaultSkill);
        skillASlot.Setup(currentEquipment.SkillA);
        skillBSlot.Setup(currentEquipment.SkillB);
    }

    private IEnumerator SetupClickAndHoldInteraction()
    {
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener(_ => holdingRoutine = new NTask(HoldingRoutine()));

        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener(_ =>
        {
            if (holdingRoutine is not null)
            {
                holdingRoutine?.Stop();

                OpenInventory();
            }

            holdingRoutine = null;
        });

        eventTrigger.triggers.Clear();
        eventTrigger.triggers.Add(pointerDown);
        eventTrigger.triggers.Add(pointerUp);
        yield break;

        IEnumerator HoldingRoutine()
        {
            yield return new WaitForSeconds(GameConstants.HOLDING_SECONDS_TO_SHOW_INFO);

            GameEvents.RaiseEquipmentInfo(currentEquipment);
            holdingRoutine = null;
        }
    }

    private void OpenInventory()
    {
        var playerData = ServiceLocator.SessionManager.PlayableCharacterData;
        GameEvents.onOpenInventory?.Invoke(new InventorySetupContext(currentEquipment.EquipmentKind, currentEquipment)
        {
            entryInteractions = new SlotClickInteractions
            {
                doubleClick = selectedEquipment =>
                {
                    playerData.CurrentEquipment.Equip(selectedEquipment);
                    UpdateEquipment(selectedEquipment);
                }
            }
        });
    }

    public class Settings
    {
        public EquipmentData equipmentData;
    }
}