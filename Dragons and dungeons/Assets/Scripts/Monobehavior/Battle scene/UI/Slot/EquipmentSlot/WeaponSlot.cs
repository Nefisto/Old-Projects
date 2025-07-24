public class WeaponSlot : Slot
{
    public override bool IsValid (ChangeItemContext ctx)
        => ctx.Item == null || (base.IsValid(ctx) && ctx.Item.data is Weapon);
}