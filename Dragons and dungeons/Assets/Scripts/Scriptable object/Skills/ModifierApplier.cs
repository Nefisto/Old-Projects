using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.ModifierApplierName, menuName = Nomenclature.ModifierApplierMenu, order = 0)]
public class ModifierApplier : Skill
{
    public override IEnumerator Act (BattleActionContext context)
    {
        yield return base.Act(context);
        
        // Simulate the animation
        yield return new WaitForSeconds(1f);
    }
}