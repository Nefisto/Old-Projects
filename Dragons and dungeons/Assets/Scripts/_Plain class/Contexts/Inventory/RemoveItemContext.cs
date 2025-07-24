public class RemoveItemContext
{
    public InventoryItem Item;

    public RemoveItemContext() { }

    public RemoveItemContext(InventoryItem item)
        => Item = item;
}