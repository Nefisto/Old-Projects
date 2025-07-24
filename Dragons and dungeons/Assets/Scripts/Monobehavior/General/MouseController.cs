using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseController : PersistentLazySingletonMonoBehaviour<MouseController>
{
    private DragContext dragContext;

    private Canvas uiCanvas;
    private GraphicRaycaster gr;

    private RectTransform iconRect;
    private Image iconImage;

    private void Start()
    {
        uiCanvas = GameObject.FindGameObjectWithTag("UI_canvas").GetComponent<Canvas>(); 
        gr = uiCanvas.GetComponent<GraphicRaycaster>();

        iconRect = (RectTransform)Instantiate(new GameObject("Icon image", typeof(RectTransform)), gr.transform).transform;
        iconRect.anchorMax = Vector2.zero;
        iconRect.anchorMin = Vector2.zero;
        iconImage = iconRect.gameObject.AddComponent<Image>();
        iconImage.enabled = false;
    }

    public void BeginDrag (BeginDragContext ctx)
    {
        EnableItemIcon(ctx);

        InitializeDragContext();
        CacheItem(ctx);
    }

    public void Dragging (DraggingContext ctx)
        => iconRect.anchoredPosition += ctx.PointerEventData.delta / uiCanvas.scaleFactor;

    public void CancelDrag()
    {
        DisableItemIcon();
        ClearDragContext();
    }
    
    public void EndDrag (EndDragContext _)
    {
        DisableItemIcon();

        if (!TryDetectSlot(out var foundSlot))
        {
            ClearDragContext();
            return;
        }

        SetEndSlot(foundSlot);

        if (!IsPossibleToChangeItems())
        {
            // Trigger fail to change event

            ClearDragContext();
            return;
        }

        ApplyChange();
    }

    private void ApplyChange()
    {
        var tempInventoryItem = new InventoryItem(dragContext.EndItemData);

        var changeResult = dragContext.EndSlot.Change(new ChangeItemContext(dragContext.InitItemData, dragContext.EndSlot));

        if (changeResult.HasStacked)
            tempInventoryItem.data = null;

        dragContext.InitSlot.Change(new ChangeItemContext(tempInventoryItem, dragContext.InitSlot));
    }

    private bool IsPossibleToChangeItems()
    {
        if (ReferenceEquals(dragContext.InitSlot, dragContext.EndSlot))
            return false;

        var newChangeContext = new ChangeItemContext
        {
            Item = dragContext.EndItemData,
            Slot = dragContext.EndSlot
        };
        if (!dragContext.InitSlot.IsValid(newChangeContext))
            return false;

        var oldChangeContext = new ChangeItemContext
        {
            Item = dragContext.InitItemData,
            Slot = dragContext.InitSlot
        };

        if (!dragContext.EndSlot.IsValid(oldChangeContext))
            return false;

        return true;
    }

    private void CacheItem (BeginDragContext ctx)
        => SetInitSlot(ctx);

    private void SetInitSlot (BeginDragContext ctx)
    {
        dragContext.InitItemData = ctx.Slot.CurrentItem;
        dragContext.InitSlot = ctx.Slot;
    }

    private void InitializeDragContext()
        => dragContext = new DragContext();

    private void SetEndSlot (Slot foundSlot)
    {
        dragContext.EndItemData = foundSlot.CurrentItem;
        dragContext.EndSlot = foundSlot;
    }

    private void ClearDragContext()
        => dragContext = new DragContext();

    private void DisableItemIcon()
    {
        iconImage.enabled = false;
        iconImage.sprite = null;
    }

    private void EnableItemIcon (BeginDragContext ctx)
    {
        iconImage.enabled = true;
        iconImage.sprite = ctx.Item.data.GetIconSprite;
        iconRect.anchoredPosition = ctx.PointerEventData.position / uiCanvas.scaleFactor;
    }

    private bool TryDetectSlot (out Slot slot)
    {
        slot = null;
        var pointerEventData = new PointerEventData(null) { position = Input.mousePosition };
        var result = new List<RaycastResult>();
        gr.Raycast(pointerEventData, result);
        if (result.Count <= 0)
            return false;

        Slot tmpSlot = null;
        var hasClickedOnSomeSlot = result
            .Select(r => r.gameObject)
            .Any(go => go.TryGetComponent(out tmpSlot));

        if (!hasClickedOnSomeSlot)
            return false;

        slot = tmpSlot;

        return true;
    }
}