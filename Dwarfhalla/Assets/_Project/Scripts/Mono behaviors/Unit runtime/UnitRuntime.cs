using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitRuntime : MonoBehaviour
{
    [FormerlySerializedAs("damageLabel")]
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text textLabel;

    [TitleGroup("References")]
    [SerializeField]
    private Transform modelFolder;

    [TitleGroup("References")]
    [SerializeField]
    private Transform modifierFolder;

    [TitleGroup("References")]
    [SerializeField]
    private ModifierEntry modifierEntry;

    private readonly Dictionary<Modifier, ModifierEntry> modifierToIconEntry = new();

    // Attack and movement renderer are used to showcase that this action is already been used
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private List<MeshRenderer> attackRenderers = new();

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private MeshRenderer movementRenderer = new();

    private NTask screenTextRoutine;

    public HealthViewController HealthController { get; set; }

    private void Awake()
    {
        HealthController = GetComponent<HealthViewController>();
    }

    private void OnDisable()
    {
        screenTextRoutine?.Stop();
    }

    public void Setup (UnitData unitData)
    {
        HideText();
        modifierFolder.DestroyChildren();
        HealthController.Setup(unitData);

        attackRenderers = modelFolder
            .GetChild(0)
            .NGetComponentsInChildren<MeshRenderer>(Extensions.Self.Exclude)
            .ToList();

        movementRenderer = modelFolder
            .GetChild(0)
            .GetComponent<MeshRenderer>();
    }

    public IEnumerator ShowDamageText (int damage, Color textColor)
    {
        if (screenTextRoutine is { IsRunning: true })
            yield return new WaitForSeconds(0.5f);

        screenTextRoutine?.Stop();
        screenTextRoutine = new NTask(ShowText(new TextSettings
        {
            text = damage.ToString(),
            textColor = textColor
        }));
    }

    public IEnumerator ShowHealingText (int healing)
    {
        screenTextRoutine?.Stop();
        screenTextRoutine = new NTask(ShowText(new TextSettings()
        {
            text = $"+{healing}",
            textColor = Color.green
        }));

        yield break;
    }

    public IEnumerator AddModifierIcon (Modifier modifier)
    {
        if (!modifier.ShowIcon)
            yield break;

        var instance = Instantiate(modifierEntry, modifierFolder, false);

        modifier.OnRefresh += () => instance.Setup(modifier);
        instance.Setup(modifier);
        modifierToIconEntry.Add(modifier, instance);
    }

    public IEnumerator RemoveModifierIcon (Modifier modifier)
    {
        if (!modifier.ShowIcon)
            yield break;

        if (!modifierToIconEntry.Remove(modifier, out var value))
        {
            Debug.LogWarning("Tried to remove modifier not added");
            yield break;
        }

        Destroy(value.gameObject);
    }

    public object DieAnimation()
    {
        var tween = modelFolder
            .transform
            .DORotate(new Vector3(90, 0, 0), 1f);

        return tween.WaitForCompletion();
    }

    public IEnumerator DisablePieceAttack()
    {
        var amountOfRenderers = attackRenderers.Count;
        foreach (var attackRenderer in attackRenderers)
        {
            attackRenderer
                .material
                .DOColor(Color.black, .5f)
                .SetEase(Ease.Linear)
                .OnComplete(() => amountOfRenderers--);
        }

        yield return new WaitUntil(() => amountOfRenderers == 0);
    }

    public IEnumerator EnablePieceAttack()
    {
        var amountOfRenderers = attackRenderers.Count;
        foreach (var attackRenderer in attackRenderers)
        {
            attackRenderer
                .material
                .DOColor(Color.white, .5f)
                .SetEase(Ease.Linear)
                .OnComplete(() => amountOfRenderers--);
        }

        yield return new WaitUntil(() => amountOfRenderers == 0);
    }

    public IEnumerator DisablePieceMovement()
    {
        yield return movementRenderer
            .material
            .DOColor(Color.black, .5f)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }

    public IEnumerator EnablePieceMovement()
    {
        yield return movementRenderer
            .material
            .DOColor(Color.white, .5f)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }

    private IEnumerator ShowText (TextSettings settings)
    {
        ShowText();
        textLabel.text = settings.text;
        textLabel.color = settings.textColor;
        yield return new WaitForSeconds(settings.timeOnScreen);
        HideText();
    }

    private void ShowText() => textLabel.gameObject.SetActive(true);
    private void HideText() => textLabel.gameObject.SetActive(false);

    public class TextSettings
    {
        public string text;

        public Color textColor = Color.black;
        public float timeOnScreen = 2f;
    }
}