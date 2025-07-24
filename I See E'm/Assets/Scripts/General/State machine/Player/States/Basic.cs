public class Basic : PlayerState
{
    public override PlayerStates playerState => PlayerStates.Basic;
    public Basic(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        
        player.Standard();
    }
}