using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class CardViewDetail : MonoBehaviour, ITooltip
{
    [TitleGroup("References")]
    [SerializeField]
    private Canvas canvas;

    [TitleGroup("References")]
    [SerializeField]
    private Image attackPatternImage;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text rangeLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text targetLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text descriptionLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text lifeLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text damageLabel;

    private NTask mouseFollowRoutine;

    private void Awake()
    {
        ServiceLocator.Tooltip = this;
        mouseFollowRoutine = new NTask(SetPositionOnMouse(), new NTask.Settings { autoStart = false });
    }

    private void Start() => HideTooltip();

    private IEnumerator SetPositionOnMouse()
    {
        var cam = canvas.worldCamera;
        var halfSize = ((RectTransform)transform).rect.size * 0.5f;
        var boxHeight = ((RectTransform)transform).rect.height;
        var boxWidth = ((RectTransform)transform).rect.width;

        while (true)
        {
            var mousePosition = Input.mousePosition;
            var offset = (Vector3)halfSize;

            if (mousePosition.y + boxHeight > Screen.height)
                offset.y *= -1;

            if (mousePosition.x + boxWidth > Screen.width)
                offset.x *= -1;

            var screenPoint = mousePosition + offset;
            var worldPoint = cam.ScreenToWorldPoint(screenPoint);
            worldPoint.z = cam.nearClipPlane;

            transform.position = worldPoint;

            yield return null;
        }
    }
}