using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class EnemyBattleActor : BattleActor
{
    // Clone reasons
    private EnemyData EnemyData => Data as EnemyData;

    private IEnumerator<Skill> skillEnumerator;
    
    #region Monobehaviour callbacks

    protected override void Awake()
    {
        base.Awake();

        CacheDialogBox();

        skillEnumerator = IterateOverSkill();
    }

    #endregion

    public void Setup (EnemyData data)
    {
        templateData = data;

        name = EnemyData.name;
        spriteRenderer.sprite = EnemyData.icon;

        skills = EnemyData.skills.ToList();
    }

    public override IEnumerator StartTurn()
    {
        yield return base.StartTurn();
    }

    public override IEnumerator RunTurn()
    {
        yield return base.RunTurn();

        yield return SetSkill();
        yield return SetTargets();
        yield return RunSkill(turnContext.skill);
    }

    [Button]
    public override IEnumerator Die()
    {
        yield return base.Die();
        
        BattleManager.Instance.AddPointsToBattleResult(new BattleResult()
        {
            beautyPoints = Random.Range(0, 100),
            madnessPoints = Random.Range(0, 100),
            experiencePoints = Random.Range(0, 100)
        });

        BattleManager.Instance.RemoveFromTurn(this);

        yield return FadeOutRoutine();

        GameEvents.Battle.RaiseCombatLogAction(new CustomCombatLog($"Enemy {EnemyData.name} has died!!"));
        
        // Addressables.ReleaseInstance(gameObject);
        Destroy(gameObject);
    }

    private IEnumerator FadeOutRoutine()
    {
        spriteRenderer.DOKill();

        var fadeTween = spriteRenderer
            .DOFade(0f, .75f);

        yield return fadeTween.WaitForCompletion();
    }
    
    private IEnumerator SetSkill()
    {
        turnContext.skill = GetNextSkill();

        yield return null;
    }

    private Skill GetNextSkill()
    {
        var testedSkills = 0;

        skillEnumerator.MoveNext();
        while (!CanPayForSkill(skillEnumerator.Current))
        {
            testedSkills++;
            skillEnumerator.MoveNext();
            
            if (testedSkills < skills.Count)
                continue;
            
            Debug.LogWarning("Enemy can pay for none of their skills, running the first one");
            return skills.First();
        }

        return skillEnumerator.Current;
    }

    private IEnumerator<Skill> IterateOverSkill()
    {
        while (true)
        {
            foreach (var skill in skills)
            {
                yield return skill;
            }
        }
    }
    
    private bool CanPayForSkill (Skill skill)
        => Mana >= skill.GetSkillCost();
}