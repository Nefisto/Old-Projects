#region Includes

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#endregion

namespace TS.PageSlider
{
    public class PageScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [Header("Configuration")]
        [SerializeField]
        private float minDeltaDrag = 0.1f;

        [SerializeField]
        private float snapDuration = 0.3f;

        [Header("Events")]
        public UnityEvent<int, int> pageChangeStarted;

        public UnityEvent<int, int> pageChangeEnded;

        private int currentPage;
        private float moveSpeed;

        private ScrollRect scrollRect;

        private float startNormalizedPosition;
        private float targetNormalizedPosition;
        private int targetPage;

        public Rect Rect
        {
            get
            {
#if UNITY_EDITOR
                if (scrollRect == null)
                    scrollRect = FindScrollRect();
#endif
                return ((RectTransform)scrollRect.transform).rect;
            }
        }

        public RectTransform Content
        {
            get
            {
#if UNITY_EDITOR
                if (scrollRect == null)
                    scrollRect = FindScrollRect();
#endif
                return scrollRect.content;
            }
        }

        private void Awake() => scrollRect = FindScrollRect();

        private void Update()
        {
            if (moveSpeed == 0)
                return;

            var position = scrollRect.horizontalNormalizedPosition;
            position += moveSpeed * Time.deltaTime;

            var min = moveSpeed > 0 ? position : targetNormalizedPosition;
            var max = moveSpeed > 0 ? targetNormalizedPosition : position;
            position = Mathf.Clamp(position, min, max);

            scrollRect.horizontalNormalizedPosition = position;

            if (Mathf.Abs(targetNormalizedPosition - position) < Mathf.Epsilon)
            {
                moveSpeed = 0;

                pageChangeEnded?.Invoke(currentPage, targetPage);

                currentPage = targetPage;
            }
        }

        public void OnBeginDrag (PointerEventData eventData)
        {
            startNormalizedPosition = scrollRect.horizontalNormalizedPosition;

            if (targetPage != currentPage)
            {
                pageChangeEnded?.Invoke(currentPage, targetPage);

                currentPage = targetPage;
            }

            moveSpeed = 0;
        }

        public void OnEndDrag (PointerEventData eventData)
        {
            var pageWidth = 1f / GetPageCount();
            var pagePosition = currentPage * pageWidth;
            var currentPosition = scrollRect.horizontalNormalizedPosition;

            // Min amount of drag to change page (normalized).
            var minPageDrag = pageWidth * minDeltaDrag;
            
            var isForwardDrag = scrollRect.horizontalNormalizedPosition > startNormalizedPosition;

            var switchPageBreakpoint = pagePosition + (isForwardDrag ? minPageDrag : -minPageDrag);

            // Change page when the current position is greater or lesser than the breakpoint,
            // when dragging forward or backward.
            var page = currentPage;
            switch (isForwardDrag)
            {
                case true when currentPosition > switchPageBreakpoint:
                    page++;
                    break;

                case false when currentPosition < switchPageBreakpoint:
                    page--;
                    break;
            }

            ScrollToPage(page);
        }

        private void ScrollToPage (int page)
        {
            targetNormalizedPosition = page * (1f / GetPageCount());
            moveSpeed = (targetNormalizedPosition - scrollRect.horizontalNormalizedPosition) / snapDuration;

            targetPage = page;

            if (targetPage != currentPage)
                pageChangeStarted?.Invoke(currentPage, targetPage);
        }

        private int GetPageCount()
        {
            var contentWidth = scrollRect.content.rect.width;
            var rectWidth = ((RectTransform)scrollRect.transform).rect.size.x;
            return Mathf.RoundToInt(contentWidth / rectWidth) - 1;
        }

        private ScrollRect FindScrollRect()
        {
            var scrollRect = GetComponentInChildren<ScrollRect>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (scrollRect == null)
                Debug.LogError("Missing ScrollRect in Children");
#endif
            return scrollRect;
        }
    }
}