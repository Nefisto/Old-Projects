using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Sand thrown",
    menuName = EditorConstants.MenuAssets.ENEMY_SKILLS + "Sand thrown", order = 0)]
public class SandThrown : Skill
{
    [TitleGroup("Settings")]
    [Range(0f, 1f)]
    [SerializeField]
    private float chanceToBlind = .5f;

    [TitleGroup("Settings")]
    [SerializeField]
    private Blind blindEffect;

    public event Action OnSuccessfullyApply;

    protected override IEnumerator Behavior (BattleActionContext context)
    {
        yield return new WaitForSeconds(1f);
    }

    protected override IEnumerator AfterCalculateActionValues (ActionInfo info)
    {
        info.effectInfo.Add(new EffectInfo
        {
            data = blindEffect,
            baseChanceToApplyEffect = chanceToBlind,
            onSuccessfullyApplied = () => OnSuccessfullyApply?.Invoke()
        });
        yield break;
    }
}