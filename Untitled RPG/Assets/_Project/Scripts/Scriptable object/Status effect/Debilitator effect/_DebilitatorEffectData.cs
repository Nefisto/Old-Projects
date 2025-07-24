using System.Collections;

public abstract class DebilitatorEffectData : StatusEffectData
{
    public override IEnumerator Setup (StatusEffectContext ctx)
    {
        yield return base.Setup(ctx);
        // TODO: change it to a modifier inside BattleActor instead of a bool 
        ShouldBlockATB = true;
    }

    public override IEnumerator End()
    {
        ShouldBlockATB = false;
        yield return base.End();
    }
}