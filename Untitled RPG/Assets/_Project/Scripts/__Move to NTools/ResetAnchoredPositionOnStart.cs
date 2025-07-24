using Sirenix.OdinInspector;
using UnityEngine;

public class ResetAnchoredPositionOnStart : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Vector2 defaultAnchoredPosition = Vector2.zero;

    private void Start() => ResetPosition();

    [DisableInEditorButton]
    private void ResetPosition()
    {
        var rectTransform = ((RectTransform)transform);
        
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        
        rectTransform.anchoredPosition = defaultAnchoredPosition;
    }
}