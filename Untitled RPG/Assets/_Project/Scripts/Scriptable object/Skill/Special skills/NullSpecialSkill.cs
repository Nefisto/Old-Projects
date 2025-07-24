using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class NullSpecialSkill : JobSkill
{
    protected override IEnumerator Behavior (BattleActionContext context)
    {
        yield break;
    }

    public override IEnumerator Run (BattleActionContext context)
    {
        yield break;
    }
}