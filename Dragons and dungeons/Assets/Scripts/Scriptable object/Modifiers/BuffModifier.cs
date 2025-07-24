using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = Nomenclature.StatusModifierName, menuName = Nomenclature.StatusModifierMenu, order = 0)]
public class BuffModifier : ModifierSO
{
    [Title("Data")]
    [SerializeField]
    private StatusEnum statusEnumToChange;

    [SerializeField]
    private int amountToChange;
    
    [MinValue(0), MaxValue(100)]
    [SerializeField]
    private int duration = 1;

    public override void Act (BattleActionContext context)
    {
        foreach (var target in context.targets)
        {
            var modifierName = "";

            switch (statusEnumToChange)
            {
                case StatusEnum.Strength:
                    modifierName = "Strength";
                    target.ApplyModifier(new StrengthModifier(amountToChange, duration));
                    break;

                case StatusEnum.Intelligence:
                    modifierName = "Intelligence";
                    target.ApplyModifier(new IntelligenceModifier(amountToChange, duration));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            GameEvents.Battle.RaiseCombatLogAction(new ModifierApplyCombatLog(target, modifierName));
        }
    }
}