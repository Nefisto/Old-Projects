using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class BattleManager : SerializedMonoBehaviour
{
    public static EntryPoint<TickContext> battleTickEntryPoint = new();

    public static EntryPoint battleStartingEntryPoint = new();
    public static EntryPoint turnBeginningEntryPoint = new();
    public static EntryPoint<BattleContext> setupBattleContextEntryPoint = new();
    public static EntryPoint<BattleSetupContext> setupingEnemiesEntryPoint = new();

    public static EntryPoint<BattleSetupContext> afterSetupBattleActorsListeners = new();

    [TitleGroup("Services")]
    [SerializeField]
    private EnemyGroupManager enemyGroupManager;

    public PlayerBattleActor player;

    [TitleGroup("References")]
    [SerializeField]
    private Camera battleCamera;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private List<BattleActor> enemies;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private float timeBetweenTicks;

    [ShowInInspector]
    private NTask battleRoutine;

    private void Start()
    {
        timeBetweenTicks = 1f / GameConstants.BATTLE_TICKS_PER_SECOND;

        GameEvents.OnPause += PauseBattle;
        GameEvents.OnUnpause += UnPauseBattle;

        GameEvents.onGameStart += () =>
        {
            ServiceLocator.MenuStack.OnOpenFirstMenu += PauseBattle;
            ServiceLocator.MenuStack.OnCloseAllMenus += UnPauseBattle;
        };

        GameEvents.onBattleTriggered += TriggeredBattleBattle;

        CloseBattleScreen();
    }

    private void OnValidate() => timeBetweenTicks = 1f / GameConstants.BATTLE_TICKS_PER_SECOND;

    private void PauseBattle() => battleRoutine?.Pause();
    private void UnPauseBattle() => battleRoutine?.Unpause();

    public static event Action OnSettingUpBattle;
    public static event Action OnSetupBattle;

    private void TriggeredBattleBattle (BattleSetupContext setupContext)
    {
        battleRoutine?.Stop();

        battleRoutine = new NTask(SetupBattle(setupContext), setupContext.autoStart);
    }

    private IEnumerator SetupBattle (BattleSetupContext ctx)
    {
        battleTickEntryPoint.Clear();

        yield return ServiceLocator.ScreenFading.FadeIn(new IScreenFading.Settings { duration = .2f });

        yield return SetupLocationModifiers(ctx);
        OnSettingUpBattle?.Invoke();

        // TODO: move these locator to setup itself on setting battle
        ServiceLocator.BattleContext = new BattleContext();
        yield return setupBattleContextEntryPoint.YieldableInvoke(ServiceLocator.BattleContext);

        ServiceLocator.TargetSelector.Clear();

        OpenBattleScreen();

        ServiceLocator.BattleResultHUD.Setup(ctx);
        ServiceLocator.BattleResulData = ScriptableObject.CreateInstance<BattleResultData>();

        yield return SetupPlayer(ctx);
        yield return setupingEnemiesEntryPoint.YieldableInvoke(ctx);
        yield return SetupEnemies(ctx);

        yield return afterSetupBattleActorsListeners.YieldableInvoke(ctx);

        OnSetupBattle?.Invoke();

        yield return ServiceLocator.ScreenFading.FadeOut(new IScreenFading.Settings { duration = .2f });

        yield return BattleRoutine(ctx);

        yield return GameEvents.OnBattleFinishingdEntryPoint?.YieldableInvoke(ServiceLocator.BattleResulData);

        yield return ServiceLocator.BattleResultHUD.Run(ServiceLocator.BattleResulData);
        yield return new WaitForSeconds(.25f);

        yield return ServiceLocator.ScreenFading.FadeIn(new IScreenFading.Settings { duration = .2f });

        CloseBattleScreen();

        yield return ServiceLocator.ScreenFading.FadeOut(new IScreenFading.Settings { duration = .2f });

        yield return GameEvents.OnBattleFinishedEntryPoint?.YieldableInvoke(ctx);
    }

    private IEnumerator BattleRoutine (BattleSetupContext _)
    {
        var delay = new WaitForSeconds(timeBetweenTicks);

        yield return battleStartingEntryPoint.YieldableInvoke();

        while (true)
        {
            yield return turnBeginningEntryPoint?.YieldableInvoke();

            if (TryGetAWinner(out var battleWinner))
            {
                ServiceLocator.BattleResulData.Winner = battleWinner;
                break;
            }

            yield return ServiceLocator.TurnController.RunQueuedActions();

            player.Tick();
            enemies.ForEach(e => e.Tick());

            yield return battleTickEntryPoint?.YieldableInvoke(new TickContext() { deltaTime = timeBetweenTicks });

            yield return delay;
        }

        yield return player.BattleFinishSetup();
        foreach (var battleActor in enemies)
            yield return battleActor.BattleFinishSetup();
    }

    private bool TryGetAWinner (out BattleResultData.BattleWinner winner)
    {
        if (enemies.Count == 0)
        {
            winner = BattleResultData.BattleWinner.Player;
            return true;
        }

        if (player.IsActorDead())
        {
            winner = BattleResultData.BattleWinner.Enemy;
            ;
            return true;
        }

        winner = BattleResultData.BattleWinner.None;
        return false;
    }

    private void OpenBattleScreen() => battleCamera.gameObject.SetActive(true);

    private void CloseBattleScreen() => battleCamera.gameObject.SetActive(false);

    private IEnumerator SetupPlayer (BattleSetupContext setupContext)
    {
        ServiceLocator.BattleContext.AddPlayer(player);

        yield return player.SetupBattleStart(new SetupBattleActorContext
        {
            data = ServiceLocator.SessionManager.PlayableCharacterData
        });
    }

    private IEnumerator SetupEnemies (BattleSetupContext setupContext)
    {
        var correctGroup = SelectCorrectEnemyGroup(setupContext.enemiesData.Count);
        correctGroup.CreateEnemiesInstance();

        yield return SetupEnemyInstances(setupContext, correctGroup);

        enemies = correctGroup
            .Cast<BattleActor>()
            .ToList();

        SetupEnemyName();

        enemies
            .ForEach(e =>
            {
                e.onLeaveCombat += () =>
                {
                    enemies.Remove(e);
                    ServiceLocator.TargetSelector.Clear();
                };

                e.onDie += ctx => ServiceLocator.BattleResulData.Add(ctx);
            });

        ServiceLocator.BattleContext.AddEnemies(enemies);
    }

    private void SetupEnemyName()
    {
        Dictionary<string, int> enemyNameToAmount = new();
        foreach (var enemyName in enemies.Select(battleActor => battleActor.ActorData.Name))
        {
            enemyNameToAmount.TryAdd(enemyName, 0);
            ++enemyNameToAmount[enemyName];
        }

        var nameSuffix = new[] { " A", " B", " C", " D", " E", " F", " G" };
        foreach (var (enemyName, amount) in enemyNameToAmount.Where(tuple => tuple.Value > 1))
        {
            var currentEnemies = enemies
                .Where(ba => ba.ActorData.Name == enemyName)
                .ToList();

            for (var i = 0; i < amount; i++)
                currentEnemies[i].ActorData.Name += nameSuffix[i];
        }

        enemies.ForEach(ba => ba.name = ba.ActorData.Name);
    }

    private IEnumerator SetupLocationModifiers (BattleSetupContext ctx)
    {
        foreach (var locationModifier in ctx.locationModifiers)
        {
            yield return locationModifier.Register();

            GameEvents.OnBattleFinishedEntryPoint += _ => StartCoroutine(locationModifier.Remove());
        }
    }

    private IEnumerator SetupEnemyInstances (BattleSetupContext setupContext, EnemyGroupSettings correctGroupSettings)
    {
        foreach (var a in correctGroupSettings.Select((enemyBattleActor, index) => new
                 {
                     enemyBattleActor,
                     index
                 }))
        {
            yield return a.enemyBattleActor.SetupBattleStart(new SetupBattleActorContext
            {
                data = setupContext.enemiesData[a.index],
                initialHealthPercentage = setupContext.enemiesData[a.index].InitialLifePercentage
            });
        }
    }

    private EnemyGroupSettings SelectCorrectEnemyGroup (int amountOfEnemies)
    {
        enemyGroupManager.DestroyAllGroups();
        var correctGroup = enemyGroupManager.GetGroupWith(amountOfEnemies);

        return correctGroup;
    }

    public class TickContext
    {
        public float deltaTime;
    }
}