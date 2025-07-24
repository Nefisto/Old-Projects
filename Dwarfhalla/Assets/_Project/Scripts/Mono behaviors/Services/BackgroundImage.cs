using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Manage a custom background image
/// </summary>
public class BackgroundImage : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image backgroundImage;

    private Canvas canvas;

    private void Awake()
    {
        ServiceLocator.BackgroundImage = this;
        canvas = backgroundImage.GetComponentInParent<Canvas>();
    }

    public IEnumerator HideImage()
    {
        backgroundImage.raycastTarget = false;
        backgroundImage.color = backgroundImage.color.SetAlpha(0f);
        yield break;
    }

    public IEnumerator ShowImage (Settings settings)
    {
        backgroundImage.raycastTarget = settings.blockRaycast;
        backgroundImage.color = settings.colorToSet.SetAlpha(settings.alphaToSet);

        canvas.sortingLayerName = SortingLayer.IDToName(settings.sortingLayerID);
        canvas.sortingOrder = settings.orderInLayer;

        yield break;
    }

    public class Settings
    {
        public float alphaToSet = 1.0f;

        /// <summary>
        ///     Should it block mouse clicks
        /// </summary>
        public bool blockRaycast = false;

        public Color colorToSet = Color.black;
        public int orderInLayer;

        public int sortingLayerID;
    }
}