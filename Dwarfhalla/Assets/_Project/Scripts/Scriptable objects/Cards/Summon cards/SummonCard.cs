using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon card", menuName = EditorConstants.CARDS_PATH + "Summon card", order = 0)]
public class SummonCard : Card
{
    public EntryPoint<object> OnSummoningFromCard { get; set; } = new();

    public enum AttackType
    {
        SingleTarget = 0,
        DamageAllTargets = 1,
    }
    
    [TitleGroup("Settings")]
    [SerializeField]
    private string cardName;

    [field: TitleGroup("Settings")]
    [field: PreviewField]
    [field: SerializeField]
    public Sprite PatternImage { get; private set; }

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public AttackType DamageType { get; private set; }
    
    [field: TitleGroup("Settings")]
    [field: Range(1, 10)]
    [field: SerializeField]
    public int Range { get; private set; } = 1;
    
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public UnitData UnitData { get; set; }

    public override string Name => cardName;

    private static BlockData TargetBlock => ServiceLocator.GameContext.TurnContext.TargetBlock;

    public override bool CanBePerformed() => TargetBlock.IsEmpty;

    public override void PreviewExecution()
    {
        TargetBlock.RuntimeBlock.Notify(CanBePerformed()
            ? RuntimeBlock.NotificationType.PossibleBlock
            : RuntimeBlock.NotificationType.ImpossibleBlock);

        CommonOperations.RunningPreviews.Add(new NTask(UnitData.PreviewAttack(new UnitData.PreviewSettings
        {
            CustomPosition = TargetBlock.Position
        })));
    }

    public override IEnumerator Perform (object context)
    {
        yield return OnSummoningFromCard?.YieldableInvoke(this);
        
        var correctContext = (context as SummonContext) ?? new SummonContext();
        
        ServiceLocator.GameContext.TurnContext.HasSuccessfullyPerformedAction = true;
        CommonOperations.CancelNotificationOnAllGrid();

        var dataInstance = UnitData.GetInstance;
        yield return dataInstance.SummonUnit(new UnitData.SummonStepSettings() { SummonedBlock = correctContext.SelectedBlock });
    }

    public class SummonContext
    {
        public BlockData SelectedBlock { get; set; } = TargetBlock;
    }
}