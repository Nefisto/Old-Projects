using System.Collections;
using System.Linq;
using Loot;
using Loot.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public partial class EnemyBattleActor : BattleActor
{
    [TitleGroup("References")]
    [SerializeField]
    private SpriteRenderer enemySprite;

    [TitleGroup("Debug")]
    [SerializeField]
    protected DropTable skillChance;

    [TitleGroup("Debug")]
    [ShowInInspector]
    protected EnemyBrain enemyBrain;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private Skill nextSkill;

    [TitleGroup("Debug")]
    [ShowInInspector]
    protected EnemyData runtimeEnemyData;

    public override ActorData ActorData => runtimeEnemyData;

    public override int GetATBGrownSpeed
        => Mathf.RoundToInt((GameConstants.MINIMUM_ATB_GROWTH + ActorData.AttackSpeed + nextSkill.SkillATBModifierBonus)
                            * cachedATBModifier);

    public override IEnumerator SetupBattleStart (SetupBattleActorContext context)
    {
        runtimeEnemyData = context.data as EnemyData;

        yield return runtimeEnemyData.Setup(new ActorData.EntityDataSetupContext { runtimeActor = this });

        enemySprite.sprite = runtimeEnemyData.Sprite;

        Currency = 0;

        yield return base.SetupBattleStart(context);

        ATBResource.OnReachMax += () =>
        {
            var action = CalculateNextAction();
            action.OnActionCast = () => ATBResource.Current = -action.skill.FatigueAmount;

            ServiceLocator.TurnController.Add(action);
        };

        enemyBrain = runtimeEnemyData.EnemyBrain.GetInstance;
        yield return SetupBehaviorRules();
        GetNewSkill();
    }

    protected virtual BattleActionContext CalculateNextAction()
    {
        if (!CanCast(nextSkill))
            nextSkill = runtimeEnemyData.DefaultAttackSkill;

        var battleAction = new BattleActionContext(this, nextSkill, nextSkill.GetSkillTargets(this));

        enemyBrain.ActUpdate();
        GetNewSkill();
        return battleAction;
    }

    private void GetNewSkill()
    {
        nextSkill = (Skill)skillChance
                        .Drop()
                        .FirstOrDefault()
                        ?.Entry
                    ?? runtimeEnemyData.DefaultAttackSkill;

        RefreshATBModifier();
    }

    public IEnumerator LeaveBattle()
    {
        onLeaveCombat?.Invoke();

        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    protected override IEnumerator Die()
    {
        yield return new WaitForSeconds(1.5f);

        onDie?.Invoke(new OnDieContext
        {
            experienceReceived = Random.Range(15, 45),
            currency = Random.Range(65, 235)
        });
        onLeaveCombat?.Invoke();

        Destroy(gameObject);
    }

    public virtual IEnumerator SetupBehaviorRules()
    {
        var drops = enemyBrain.GetBehaviorRules(new EnemyBrain.BehaviorContext()
        {
            battleActor = this,
            enemyData = runtimeEnemyData
        });

        foreach (var drop in drops)
        {
            yield return (drop.Entry as Skill).Setup();
        }
        
        CreateASkillBehavior(drops);
    }

    private void CreateASkillBehavior (params Drop[] drops)
    {
        skillChance = ScriptableObject.CreateInstance<DropTable>();
        skillChance.name = $"{name}_Skills";
        skillChance.PercentageCalculation = PercentageCalculation.Weighted;

        foreach (var drop in drops)
            skillChance.AddDrop(drop);
    }

    private bool CanCast (Skill skill) => true;
}