public class HeadSlot : EquipmentSlot
{
    public override bool IsValid (ChangeItemContext ctx)
        => ctx.Item == null || (base.IsValid(ctx) && ctx.Item.data is HeadArmor);
}