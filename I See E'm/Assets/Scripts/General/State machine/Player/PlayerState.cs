public abstract class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;

    abstract public PlayerStates playerState { get; }
        
    public PlayerState(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void HandleInput()
    {
        
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void GizmosUpdate()
    {
        
    }
    
    public virtual void Exit()
    {

    }

    public virtual void CastFireball()
    {
        var nearestEnemy = player.GetNearestEnemy();
        
        if (nearestEnemy)
            player.CastFireballAtTarget(nearestEnemy);
        else
            player.CastFireballAtDirection(player.FlashlightPosition.forward);
    }

    public virtual void CastRay()
    {
        var target = player.GetNearestEnemy();

        if (!target)
        {
            player.RaiseFailedToCastRay();
            return;
        }

        player.RayTarget = target;
        player.CastRayAtTarget();
    }
}