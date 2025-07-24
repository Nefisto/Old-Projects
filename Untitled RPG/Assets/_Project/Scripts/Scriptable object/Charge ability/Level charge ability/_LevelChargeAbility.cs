using System.Collections;

public abstract class LevelChargeAbility : ChargeAbility
{
    public abstract IEnumerator ApplyAbility (BattleActionContext context);
}