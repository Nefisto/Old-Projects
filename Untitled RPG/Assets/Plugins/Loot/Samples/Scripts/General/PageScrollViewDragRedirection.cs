using Sample;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OldSample
{
    // This exist only to allow you to drag scroll view after click on some button, because by default buttons and panels will override scroll view drag behaviour
    /// <summary>
    ///     This will allow you to drag scroll view without trigger
    /// </summary>
    public class PageScrollViewDragRedirection : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        // To enable drag clicking on items in HUD
        public ScrollRect scrollRect;

        /// <summary>
        ///     We need a reference to the scroll rect that this item is inside.
        ///     If entries are not inserted in runtime, we can just drag n drop on inspector.
        /// </summary>
        public ScrollRect GetScrollRect => scrollRect == null
            ? scrollRect = transform.root.GetComponent<HUDManager>().refToPageScrollRect
            : scrollRect;


        public void OnBeginDrag (PointerEventData eventData)
        {
            GetScrollRect.OnBeginDrag(eventData);

            SampleSettings.DisableHover();
        }

        public void OnDrag (PointerEventData eventData)
            => GetScrollRect.OnDrag(eventData);

        public void OnEndDrag (PointerEventData eventData)
        {
            GetScrollRect.OnEndDrag(eventData);

            SampleSettings.EnableHover();
        }
    }
}