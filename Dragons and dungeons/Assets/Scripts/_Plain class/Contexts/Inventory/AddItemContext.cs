public class AddItemContext
{
    public InventoryItem Item;

    public AddItemContext() { }

    public AddItemContext(InventoryItem item)
        => Item = item;
}