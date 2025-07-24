public partial class FriendlyBattleActor
{
    public override void Heal (BattleActionContext context)
    {
        base.Heal(context);
        
        // Run animation
        BlinkFeedback();
    }
}