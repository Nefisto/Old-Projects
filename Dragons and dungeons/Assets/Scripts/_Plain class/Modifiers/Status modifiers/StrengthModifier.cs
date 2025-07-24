using UnityEngine;

public class StrengthModifier : StatusModifier
{
    public override Sprite Icon => PersistentManager.Instance.GetIcon(ModifierKind.Strength);
    
    public StrengthModifier(int amountToModify, int turnsDuration) : base(amountToModify, turnsDuration) { }

    public override void Apply (ModifierContext modifierContext)
    {
        modifierContext.target.Data.AddTemporaryStatus(StatusEnum.Strength, amountToModify);
    }

    public override void Remove (ModifierContext modifierContext)
    {
        modifierContext.target.Data.AddTemporaryStatus(StatusEnum.Strength, -amountToModify);
    }
}