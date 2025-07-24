using Loot;
using UnityEngine;

[CreateAssetMenu(fileName = "Dummy brain", menuName = EditorConstants.MenuAssets.ENEMY_BRAIN + "Dummy",
    order = 0)]
public class DummyBrain : EnemyBrain
{
    private Drop attackDrop;

    public override Drop[] GetBehaviorRules (BehaviorContext context)
    {
        var attackInstance = context.enemyData.DefaultAttackSkill.GetInstance;
        attackDrop = new Drop
        {
            Entry = attackInstance,
            Weight = 1
        };

        return new[] { attackDrop };
    }
}