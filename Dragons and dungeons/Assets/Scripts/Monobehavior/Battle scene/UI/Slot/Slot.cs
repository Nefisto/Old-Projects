using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : SerializedMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Title("Settings")]
    [SerializeField]
    protected float disableFadeValue = .45f;

    [SerializeField]
    protected float enableFadeValue = 1f;
    
    [Title("Control")]
    [SerializeField]
    protected Image iconImage;

    [SerializeField]
    protected Image borderImage;
    
    [SerializeField]
    protected TextMeshProUGUI amountText;
    
    [Title("Debug")]
    [ReadOnly]
    [field: SerializeField]
    public InventoryItem CurrentItem { get; protected set; }

    [OdinSerialize]
    public int Index { get; private set; }

    [ReadOnly]
    public IDroppableArea droppableArea;

    private void Start()
    {
        if (CurrentItem != null)
            UpdateSlot(CurrentItem);
    }

    /// <summary>
    /// Fade out images and disable raycast
    /// </summary>
    [Button]
    public virtual void DisableSlot()
    {
        var borderColor = borderImage.color;
        var iconColor = iconImage.color;
        
        borderImage.color = new Color(borderColor.r, borderColor.g, borderColor.b, disableFadeValue);
        borderImage.raycastTarget = false;
        iconImage.color = new Color(iconColor.r, iconColor.g, iconColor.b, disableFadeValue);
        iconImage.raycastTarget = false;
    }

    [Button]
    public virtual void EnableSlot()
    {
        var borderColor = borderImage.color;
        var iconColor = iconImage.color;
        
        borderImage.color = new Color(borderColor.r, borderColor.g, borderColor.b, enableFadeValue);
        borderImage.raycastTarget = true;
        iconImage.color = new Color(iconColor.r, iconColor.g, iconColor.b, enableFadeValue);
        iconImage.raycastTarget = true;
    }
    
    public void SetPosition (int position)
        => Index = position;

    [DisableInEditorMode]
    [Button]
    public void UpdateSlot (InventoryItem item)
    {
        CurrentItem = item;
        
        if (IsEmpty())
        {
            ClearSlot();
            return;
        }
        
        InnerUpdateSlot(item);
    }

    protected virtual void InnerUpdateSlot (InventoryItem item)
    {
        iconImage.sprite = CurrentItem.data.GetIconSprite;
        iconImage.enabled = true;
    }
     
    [DisableInEditorMode]
    [Button]
    public virtual void ClearSlot()
    {
        CurrentItem = null;
        iconImage.enabled = false;
    }

    public virtual ChangeResult Change (ChangeItemContext ctx)
        => droppableArea.Change(ctx);
    
    public virtual bool IsValid (ChangeItemContext ctx)
        => droppableArea.IsValid(ctx);

    public void OnBeginDrag (PointerEventData eventData)
    {
        if (!IsValidToDrag(eventData))
            return;
        
        MouseController.Instance.BeginDrag(new BeginDragContext()
        {
            Slot = this,
            PointerEventData = eventData,
            Item = CurrentItem
        });
    }

    public void OnDrag (PointerEventData eventData)
    {
        if (!IsValidToDrag(eventData))
            return;
        
        MouseController.Instance.Dragging(new DraggingContext(){PointerEventData = eventData});
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        if (!IsValidToDrag(eventData))
            return;
        
        MouseController.Instance.EndDrag(new EndDragContext(){PointerEventData = eventData});
    }

    private bool IsValidToDrag(PointerEventData eventData)
        => CurrentItem != null && eventData.button == PointerEventData.InputButton.Left;

    public void OnPointerClick (PointerEventData eventData)
    {
        if (!IsRightClick(eventData))
            return;

        if (IsEmpty())
            return;
        
        InfoCardPanel.Instance.ShowInfoCard(new InfoCardContext
        {
            Title = CurrentItem.data.itemName.ToUpper(),
            Icon = CurrentItem.data.GetIconSprite,
            Description = CurrentItem.data.description
        });
    }

    protected bool IsEmpty()
        => CurrentItem == null || CurrentItem.data == null;

    private static bool IsRightClick(PointerEventData eventData)
        => eventData.button == PointerEventData.InputButton.Right;
}