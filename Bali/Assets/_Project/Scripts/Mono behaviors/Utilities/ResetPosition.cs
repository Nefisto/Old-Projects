using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    private void Start()
        => ((RectTransform)transform).anchoredPosition = Vector2.zero;
}