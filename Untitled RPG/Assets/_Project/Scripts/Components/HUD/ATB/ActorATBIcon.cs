using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ActorATBIcon : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image atbIcon;

    [TitleGroup("References")]
    [SerializeField]
    private GameObject tiredIcon;

    [TitleGroup("References")]
    [SerializeField]
    private NDictionary<StatusEffectKind, GameObject> effectKindToObject;

    /// <summary>
    /// Just to avoid casting transform at every access
    /// </summary>
    [TitleGroup("References")]
    [SerializeField]
    private RectTransform rectTransform;

    public IEnumerator Setup (Settings settings)
    {
        atbIcon.sprite = settings.battleActor.ActorData.ATBIcon;

        rectTransform.anchoredPosition = CalculateAnchoredPosition(settings);
        settings.battleActor.ATBResource.OnUpdatedCurrent += (_, _)
            => rectTransform.anchoredPosition = CalculateAnchoredPosition(settings);
        
        foreach (var (_, go) in effectKindToObject)
            go.SetActive(false);

        yield break;
    }

    public void EnableSubIcon (StatusEffectKind kind) => effectKindToObject[kind].SetActive(true);

    public void DisableSubIcon (StatusEffectKind kind) => effectKindToObject[kind].SetActive(false);

    private Vector2 CalculateAnchoredPosition (Settings settings)
    {
        var percentage = settings.battleActor.ATBResource.CurrentPercentage;

        tiredIcon.SetActive(settings.battleActor.IsOnFatigue);

        var negativeSize = settings.markZero.anchoredPosition.x;
        var positiveSize = settings.atbBarSize - negativeSize;
        return percentage > 0f
            ? new Vector2(settings.markZero.anchoredPosition.x + positiveSize * percentage, 0f)
            : new Vector2(negativeSize * (1 - Mathf.Abs(percentage)), 0f);
    }

    public class Settings
    {
        /// <summary>
        /// This is used to allow the actor to calculate their own position based on parent bar size
        /// </summary>
        public float atbBarSize;

        public BattleActor battleActor;

        public RectTransform markZero;
    }
}