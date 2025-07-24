using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class PatternGetSettings
{
    public Vector2Int position;
    public int range = 2;
}

public abstract class UnitData : ScriptableObject
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public string Name { get; private set; }

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public int UnitWorth { get; private set; } = 1;

    [field: TitleGroup("Settings")]
    [field: Range(1, 10)]
    [field: SerializeField]
    public int MaxHealth { get; protected set; } = 1;

    [field: TitleGroup("Settings")]
    [field: Multiline(5)]
    [field: SerializeField]
    public string Description { get; set; }

    [field: TitleGroup("Settings")]
    [field: Range(1, 5)]
    [field: SerializeField]
    public int Damage { get; protected set; } = 1;

    [field: TitleGroup("Settings")]
    [field: Range(1, 5)]
    [field: SerializeField]
    public int DamageRange { get; protected set; } = 2;

    [field: TitleGroup("Settings")]
    [field: Range(1, 10)]
    [field: SerializeField]
    public int CoinReward { get; protected set; } = 1;

    [field: TitleGroup("References")]
    [field: SerializeField]
    public UnitRuntime ModelPrefab { get; private set; }

    private int currentHealth;

    [TitleGroup("Debug")]
    [ShowInInspector]
    private bool hasAttackedThisTurn;

    [TitleGroup("Debug")]
    [ShowInInspector]
    private bool hasBeenSummonedThisTurn;

    [TitleGroup("Debug")]
    [ShowInInspector]
    private bool hasMovedThisTurn;

    public Action OnUpdateHealth;

    public EntryPoint<object> OnTakingDamage { get; set; } = new();
    public EntryPoint<object> OnTookDamage { get; set; } = new();


    /// <summary>
    ///     The unit itself does not have a "turn start" concept, but the owner of this piece should call this for their pieces
    ///     at a turn start
    /// </summary>
    public EntryPoint<object> OnTurnStart { get; set; } = new();

    public Action OnDie { get; set; }

    protected TurnContext TurnContext => ServiceLocator.GameContext.TurnContext;

    public UnitData GetInstance => Instantiate(this);


    [TitleGroup("Settings")]
    [ShowInInspector]
    public abstract UnitSide UnitSide { get; }

    [TitleGroup("Settings")]
    [ShowInInspector]
    public abstract UnitKind UnitKind { get; }

    public int CurrentHealth
    {
        get => currentHealth;
        protected set
        {
            currentHealth = value;
            OnUpdateHealth?.Invoke();
        }
    }

    [field: TitleGroup("Debug")]
    [field: HideInEditorMode]
    [field: ShowInInspector]
    public List<Modifier> Modifiers { get; protected set; } = new();

    public bool CanAttack => !hasAttackedThisTurn;
    public bool CanMove => !hasMovedThisTurn;

    public bool IsDead => CurrentHealth <= 0;

    public UnitRuntime RuntimeUnit { get; set; }

    protected Vector2Int PiecePosition => BlockData.Position;

    public BlockData BlockData { get; set; }

    public virtual bool HasEnemyOnFieldOfView
        => CommonOperations
            .GetVisibleBlocks(new PatternGetSettings { position = PiecePosition }, GetDamagePattern)
            .Any();

    public abstract Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern { get; }

    public virtual List<Vector2Int> GetDamagePosition
        => GetDamagePattern(new PatternGetSettings
            {
                position = PiecePosition,
                range = DamageRange
            })
            .ToList();

    public bool HasExtraLife => CurrentHealth > MaxHealth;
    public bool CanTakeDamage => Modifiers.All(m => m.Kind != ModifierKind.Shield);

    public virtual void Setup (BlockData blockData)
    {
        CurrentHealth = MaxHealth;
        BlockData = blockData;
        RuntimeUnit.Setup(this);
    }

    public List<Vector2Int> GetPositionInView()
    {
        var result = new List<Vector2Int>();

        var directionToPositions = GetDamagePattern(new PatternGetSettings { position = PiecePosition })
            .ToDirectForPosition(PiecePosition);

        foreach (var (_, positions) in directionToPositions)
        {
            foreach (var currentPosition in positions)
            {
                result.Add(currentPosition);

                if (CommonOperations.GetBlockDataAt(currentPosition).HasUnitOnIt)
                    break;
            }
        }

        return result;
    }

    protected List<ForeseeActionResult> GetDamageForeseeActions (Vector2Int foreseePosition)
        => GetDamagePattern(new PatternGetSettings { position = foreseePosition })
            .GetBlocksFromPositions()
            .Where(bd => bd.HasDwarfUnit)
            .Select(bd => new DamageActionResult()
            {
                target = bd.UnitData,
                willTakeDamage = bd.UnitData.Modifiers.All(m => m.Kind != ModifierKind.Shield),
                willKill = bd.UnitData.Modifiers.All(m => m.Kind != ModifierKind.Shield)
                           && bd.UnitData.CurrentHealth <= Damage
            })
            .Cast<ForeseeActionResult>()
            .ToList();

    public List<UnitData> GetEnemiesInView()
        => GetPositionInView()
            .Select(CommonOperations.GetBlockDataAt)
            .Where(bd => bd.HasUnitOnIt && bd.UnitData.UnitSide != UnitSide)
            .Select(bd => bd.UnitData)
            .ToList();

    public virtual IEnumerator PreviewAttack (PreviewSettings settings = null)
    {
        GetDamagePattern(new PatternGetSettings { position = settings?.CustomPosition ?? PiecePosition })
            .GetBlocksFromPositions()
            .ForEach(rb => rb.RuntimeBlock.Notify(RuntimeBlock.NotificationType.Damage));

        yield break;
    }

    public virtual IEnumerator TurnStart()
    {
        hasBeenSummonedThisTurn = false;
        hasAttackedThisTurn = false;
        hasMovedThisTurn = false;
        yield return OnTurnStart?.YieldableInvoke();
    }

    public IEnumerator TurnEnd()
    {
        RuntimeUnit.StartCoroutine(RuntimeUnit.EnablePieceAttack());
        RuntimeUnit.StartCoroutine(RuntimeUnit.EnablePieceMovement());
        yield break;
    }

    public virtual IEnumerator OnKillBehavior()
    {
        yield break;
    }

    public virtual IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        ServiceLocator.GlobalNotifyObject.SetActive(true);
        settings ??= new PerformAttackSettings();

        if (settings.isSummoningAttack)
            yield break;

        if (hasBeenSummonedThisTurn)
            yield return BlockData.ApplyDamage(new BlockData.ApplyDamageSettings { damage = 1 });

        hasAttackedThisTurn = true;
        RuntimeUnit.StartCoroutine(RuntimeUnit.DisablePieceAttack());
        ServiceLocator.GlobalNotifyObject.SetActive(false);
    }

    public IEnumerator SummonUnit (SummonStepSettings settings)
    {
        RuntimeUnit = Instantiate(ModelPrefab);
        RuntimeUnit.transform.rotation = ModelPrefab.transform.rotation;

        Setup(settings.SummonedBlock);
        settings.SummonedBlock.SetUnit(this);

        yield return SummonStep(settings);
    }

    protected virtual IEnumerator SummonStep (SummonStepSettings settings)
    {
        hasBeenSummonedThisTurn = true;

        yield return PerformAttack(new PerformAttackSettings { isSummoningAttack = true });
    }

    public void ConsumeMovement()
    {
        hasMovedThisTurn = true;
        RuntimeUnit.StartCoroutine(RuntimeUnit.DisablePieceMovement());
    }

    public IEnumerator TakeDamage (BlockData.ApplyDamageSettings settings)
    {
        yield return OnTakingDamage?.YieldableInvoke(settings);

        CurrentHealth = Mathf.Max(0, CurrentHealth - settings.damage);

        yield return OnTookDamage?.YieldableInvoke(settings);

        if (settings.showDamageTest)
            yield return RuntimeUnit.ShowDamageText(settings.damage, settings.damageColor);

        if (!IsDead)
            yield break;

        settings.onDieCallback?.Invoke();
        yield return Die();
    }

    protected virtual IEnumerator Die()
    {
        OnDie?.Invoke();

        foreach (var modifier in Modifiers.ToList())
            yield return modifier.Remove();

        GameManager.DeadUnits.Add(this);
    }

    public IEnumerator DieAnimation()
    {
        yield return RuntimeUnit.DieAnimation();
        Destroy(RuntimeUnit.gameObject);

        if (UnitSide != UnitSide.Goblin)
            yield break;

        yield return GameEntryPoints.OnGoblinDied?.YieldableInvoke(this);

        var coin = ServiceLocator.CoinPooler.GetNewCoin();
        coin.Transform.position = BlockData.RuntimeBlock.PiecePosition.position + new Vector3(0f, 1f, 0f);

        var task = new NTask(coin.Animate(new CoinAnimation.Settings { coinAmount = CoinReward }));
        task.OnFinished += _ => Destroy(coin.Transform.gameObject);
    }

    public IEnumerator GetHealed (BlockData.ApplyHealingSettings settings)
    {
        CurrentHealth += settings.healing;
        yield return RuntimeUnit.ShowHealingText(settings.healing);
    }

    public bool HasModifier (ModifierKind kind) => Modifiers.Any(m => m.Kind == kind);

    public IEnumerator ApplyModifier (ModifierSettings settings)
    {
        var modifier = settings.modifier;
        if (Modifiers.Contains(modifier))
        {
            yield return Modifiers
                .First(m => modifier == m)
                .Reapply(settings);

            yield break;
        }

        Modifiers.Add(modifier);
        yield return modifier.Apply(settings);

        modifier.OnRemoved += RemoveModifier;
        yield return RuntimeUnit.AddModifierIcon(modifier);
    }

    public virtual ForeseeAction GetForeseeActions (Vector2Int foreseePosition)
    {
        var foreseeAction = new ForeseeAction();

        var possibleDamageActions = GetDamageForeseeActions(foreseePosition);

        if (possibleDamageActions.IsEmpty())
            return foreseeAction;

        var amountOfPossibleDamageActions = possibleDamageActions.Count;
        foreach (var damageActionResult in possibleDamageActions.Cast<DamageActionResult>())
            damageActionResult.chanceToHit = 1f / amountOfPossibleDamageActions;

        if (!foreseeAction.actionTypeToTargets.TryAdd(ActionType.SingleDamage, possibleDamageActions))
            foreseeAction.actionTypeToTargets[ActionType.SingleDamage] = possibleDamageActions;

        return foreseeAction;
    }

    private IEnumerator RemoveModifier (object caller)
    {
        Modifiers.Remove(caller as Modifier);
        yield return RuntimeUnit.RemoveModifierIcon(caller as Modifier);
        (caller as Modifier).OnRemoved -= RemoveModifier;
    }

    public class PreviewSettings
    {
        public Vector2Int? CustomPosition { get; set; } = null;
    }

    public class SummonStepSettings
    {
        public BlockData SummonedBlock { get; set; }
    }

    public class PerformAttackSettings
    {
        public bool isSummoningAttack;
    }
}