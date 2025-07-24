using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChargeLevelBar : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image bar;

    [FormerlySerializedAs("instantIconsManager")]
    [TitleGroup("References")]
    [SerializeField]
    private HoldingIconManager holdingIconManager;

    public IEnumerator Setup (Skill.ChargeLevelSettings chargeSettings)
    {
        bar.color = Color.red;
        Fill(0f);
        yield return holdingIconManager.Setup(chargeSettings);
    }

    public void Fill (float percentage)
    {
        bar.fillAmount = percentage;
        if (percentage.IsNearlyEnoughTo(1f))
            bar.color = Color.green;
    }
}