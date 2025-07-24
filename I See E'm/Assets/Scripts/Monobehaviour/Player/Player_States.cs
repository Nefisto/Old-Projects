using System;

public partial class Player
{
    private PlayerStates CurrentState => stateMachine.CurrentState.playerState;
    
    private PlayerStateMachine stateMachine;

    private Basic basic;
    private Aiming aiming;
    
    public void ChangeState(PlayerStates state)
    {
        PlayerState targetState = state switch
        {
            PlayerStates.Basic => basic,
            PlayerStates.Aiming => aiming,
            _ => throw new Exception("Unknown player state")
        };

        stateMachine.ChangeState(targetState);
    }
    
    private void SetupStateMachine()
    {
        stateMachine = new PlayerStateMachine();

        basic = new Basic(this, stateMachine);
        aiming = new Aiming(this, stateMachine);
        
        stateMachine.Initialize(basic);
    }
}