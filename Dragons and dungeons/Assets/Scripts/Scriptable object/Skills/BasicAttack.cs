using System.Collections;
using UnityEngine;

/// <summary>
/// Damage target based on function passed by context, otherwise 
/// </summary>
[CreateAssetMenu(fileName = Nomenclature.BasicAttackName, menuName = Nomenclature.BasicAttackMenu, order = 0)]
public class BasicAttack : Skill
{
    public override IEnumerator Act (BattleActionContext battleActionContext)
    {
        yield return base.Act(battleActionContext);

        battleActionContext.actionValue = CalculateDamage(battleActionContext.caster.Data);

        foreach (var target in battleActionContext.targets)
        {
            yield return target.TakeDamage(battleActionContext);
        }
        
        // Simulate the animation
        yield return SkillDelay(.3f);
    }

    /// <summary>
    /// This will allow us to modify the damage in any way that we want.
    /// e.g. If we want to create a (math)function that will give damage based on a pre selected data,
    ///     all that we must do is create a field with the main data and apply the method here.
    /// By default we will keep with str 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private int CalculateDamage(ActorData data)
        => data.GetCurrentStatus().Strength;
}