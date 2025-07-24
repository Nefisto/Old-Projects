public abstract class EquipmentSlot : Slot
{
    public override bool IsValid (ChangeItemContext ctx)
        => ctx.Item.data is EquipmentData && droppableArea.IsValid(ctx);
}