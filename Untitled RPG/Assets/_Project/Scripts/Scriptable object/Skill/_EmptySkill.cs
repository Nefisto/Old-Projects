using System.Collections;

public class EmptySkill : Skill
{
    public override IEnumerator Run (BattleActionContext context)
    {
        yield return null;
    }

    protected override IEnumerator Behavior (BattleActionContext context)
    {
        yield return null;
    }
}