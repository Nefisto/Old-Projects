using System.Collections;

public abstract class SingleChargeAbility : ChargeAbility
{
    public abstract IEnumerator RegisterAbility();
    public abstract IEnumerator RemoveAbility();
}