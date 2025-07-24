using UnityEngine;

public class IntelligenceModifier : StatusModifier
{
    public override Sprite Icon => PersistentManager.Instance.GetIcon(ModifierKind.Intelligence);
    
    public IntelligenceModifier(int amountToModify, int turnsDuration) : base(amountToModify, turnsDuration) { }
    
    public override void Apply (ModifierContext modifierContext)
    {
        modifierContext.target.Data.AddTemporaryStatus(StatusEnum.Intelligence, amountToModify);
    }

    public override void Remove (ModifierContext modifierContext)
    {
        modifierContext.target.Data.AddTemporaryStatus(StatusEnum.Intelligence, -amountToModify);
    }
}