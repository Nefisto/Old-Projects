using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.HealName, menuName = Nomenclature.HealMenu, order = 0)]
public class Heal : Skill
{
    [Title("Data")]
    public int baseAmount;
    
    public override IEnumerator Act (BattleActionContext context)
    {
        yield return base.Act(context);
        
        context.originalActionValue = baseAmount;

        foreach (var target in context.targets)
        {
            context.actionValue = CalculateHealAmount(target.Data);
            target.Heal(context);
        }
        
        // Simulate the animation
        yield return new WaitForSeconds(.5f);
    }

    private int CalculateHealAmount (ActorData target)
    {
        return Mathf.RoundToInt(baseAmount + 2.5f * target.GetCurrentStatus().Intelligence);
    }
}