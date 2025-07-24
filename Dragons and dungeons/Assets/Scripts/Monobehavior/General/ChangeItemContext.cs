public class ChangeItemContext
{
    public InventoryItem Item;
    public Slot Slot;

    public bool HasEmptyItem => Item == null || Item.data == null;

    public ChangeItemContext() { }

    public ChangeItemContext (InventoryItem item, Slot slot)
    {
        Item = item;
        Slot = slot;
    }
}