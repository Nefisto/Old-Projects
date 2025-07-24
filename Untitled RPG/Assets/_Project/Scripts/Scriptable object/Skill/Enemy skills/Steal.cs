using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Steal money",
    menuName = EditorConstants.MenuAssets.ENEMY_SKILLS + "Steal money", order = 0)]
public class Steal : Skill
{
    [TitleGroup("Tab/Common/Settings")]
    [Range(0f, 1f)]
    [SerializeField]
    private float percentageStole = 0.05f;
    
    public event Action<int> OnSuccessfullyStole;
    
    public override IEnumerator Setup()
    {
        yield return base.Setup();

        OnBeginningAnimationEntryPoint += BeginningAnimationHandle;
    }

    protected override IEnumerator BeforeCalculateActionValues (BattleActionContext context)
    {
        yield return base.BeforeCalculateActionValues(context);
        
        foreach (var actionInfo in context)
        {
            if (actionInfo.target is not PlayerBattleActor playerActor)
                yield break;

            actionInfo.moneyStole = Mathf.RoundToInt(playerActor.RuntimeData.CurrentCurrency * percentageStole);
        }
    }

    protected override IEnumerator Behavior (BattleActionContext context)
    {
        foreach (var actionInfo in context)
        {
            if (actionInfo.target is not PlayerBattleActor playerActor)
                yield break;
            
            playerActor.RuntimeData.RemoveCurrency(actionInfo.moneyStole);
            context.caster.Currency += actionInfo.moneyStole;

            OnSuccessfullyStole?.Invoke(actionInfo.moneyStole);
        }
    }

    private IEnumerator BeginningAnimationHandle (BattleActionContext ctx)
    {
        foreach (var actionInfo in ctx)
        {
            if (actionInfo.moneyStole == 0)
                ServiceLocator.FloatText.MissText(actionInfo.caster.transform);
            
            ServiceLocator.FloatText.AddCustomFloatText(new FloatTextSettings(
                $"_{actionInfo.moneyStole}",
                actionInfo.target.transform,
                textColor: Color.yellow));
        }

        yield break;
    }
}