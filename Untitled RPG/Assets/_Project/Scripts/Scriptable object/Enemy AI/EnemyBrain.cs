using Loot;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EnemyBrain : ScriptableObject
{
    [TitleGroup("Debug", order: 100)]
    [ReadOnly]
    [SerializeField]
    protected NDictionary<string, Drop> skillNameToRule;

    public virtual EnemyBrain GetInstance => Instantiate(this);

    public abstract Drop[] GetBehaviorRules (BehaviorContext context);

    public virtual void ActUpdate() { }

    public class BehaviorContext
    {
        public BattleActor battleActor;
        public EnemyData enemyData;
    }
}