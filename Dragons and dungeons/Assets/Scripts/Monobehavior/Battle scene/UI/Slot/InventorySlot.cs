using UnityEngine;

public class InventorySlot : Slot
{
    protected override void InnerUpdateSlot (InventoryItem item)
    {
        base.InnerUpdateSlot(item);
        
        amountText.text = Mathf.Clamp(CurrentItem.amount, 0, GameConstants.General.MaxItemStack).ToString();
    }
    
    public override void ClearSlot()
    {
        base.ClearSlot();
        
        amountText.text = "-";
    }
}