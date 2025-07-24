using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GradientBar : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Gradient colorGradient;

    [TitleGroup("Settings")]
    [SerializeField]
    private Gradient recoveryGradient;

    [TitleGroup("References")]
    [SerializeField]
    private Image bar;

    [TitleGroup("Debug")]
    [Range(0f, 1f)]
    [DisableInEditorMode]
    [SerializeField]
    private float fillAmount = 0f;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private Gradient currentGradient;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private float lastPercentage;

    protected virtual void Start() => UpdateGradient(colorGradient);

    private void OnValidate()
    {
        if (!Application.isPlaying)
            return;

        bar.fillAmount = fillAmount;
        UpdateBar(fillAmount);
    }

    public void UpdateGradient (Gradient gradient)
    {
        currentGradient = gradient;
    }

    [Title("Debug")]
    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    public virtual void UpdateBar (float percentage)
    {
        UpdateFillAmount(percentage);
        UpdateColor(percentage);

        lastPercentage = percentage;
    }

    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    public void ChangeBarColor (Color color)
    {
        currentGradient = new Gradient
        {
            colorKeys = new[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) }
        };
        UpdateBar(lastPercentage);
    }

    public IEnumerator RecoveryMode (float seconds)
    {
        currentGradient = recoveryGradient;

        var remainingTime = seconds;
        while (remainingTime >= 0f)
        {
            UpdateBar(remainingTime / seconds);

            remainingTime -= Time.deltaTime;
            yield return null;
        }

        currentGradient = colorGradient;
    }

    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    public void ResetToDefaultGradient()
    {
        currentGradient = colorGradient;
        UpdateBar(lastPercentage);
    }

    protected virtual void UpdateColor (float percentage)
    {
        var color = currentGradient.Evaluate(percentage);
        PaintBar(color);
    }

    protected void PaintBar (Color32 color)
    {
        bar.color = color;
    }

    private void UpdateFillAmount (float percentage) => bar.fillAmount = percentage;
}