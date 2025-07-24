using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class BattleManager : SingletonMonoBehaviour<BattleManager>
{
    [Title("Control")]
    [SerializeField]
    private FadeBackground background;

    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;

        var _ = MouseController.Instance;
    }

    private void OnEnable()
        => RegisterOnGameEvents();

    private void OnDisable()
        => UnregisterOnGameEvents();

    public event Action OnSpawnActors;

    private void RegisterOnGameEvents()
    {
        GameEvents.Battle.OnSetupBattle += SetupBattle;
        GameEvents.Battle.OnBattleStart += BattleStart;
    }

    private void UnregisterOnGameEvents()
    {
        GameEvents.Battle.OnSetupBattle -= SetupBattle;
        GameEvents.Battle.OnBattleStart -= BattleStart;
    }

    public void SetupBattle (BattleEncounterContext context)
    {
        StartCoroutine(_SetupBattle());

        IEnumerator _SetupBattle()
        {
            background.SetBackgroundAlpha(1f);
            SpawnEnemies(context.enemyGroup);
            SpawnPlayerCharacters(context.player);
            ResetBattleResults();
            
            OnSpawnActors?.Invoke();
            
            yield return new WaitForSeconds(1f);
            GameEvents.Battle.RaiseBattleStart();
        }
    }
}