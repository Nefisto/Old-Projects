using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class DungeonManager : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField]
    private AssetReference player;

    [SerializeField]
    private AssetReference battleScene;

    [SerializeField]
    private Transform initialPosition;

    [SerializeField]
    private DungeonNode initialNode;

    [SerializeField]
    private FadeBackground fadeBackground;

    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private Pawn pawn;

    [ReadOnly]
    [SerializeField]
    private List<DungeonNode> dungeonNodes;

    private void Awake()
    {
        CacheNodes();
        CachePawn();
    }

    private IEnumerator Start()
    {
        yield return player.LoadAssetAsync<Player>();

        GameEvents.DungeonMap.RaiseFirstSetupDungeon();
        yield return new WaitForSeconds(1f);
        
        GameEvents.DungeonMap.RaiseStartDungeon();
    }

    private void OnEnable()
        => RegisterOnGameEvents();

    private void OnDisable()
        => UnregisterOnGameEvents();

    private IEnumerator StartDungeon()
    {
        yield return fadeBackground.FadeOutRoutine();
        yield return WalkToFirstNode();
    }

    private object WalkToFirstNode()
        => pawn.GoToNode(initialNode);

    private void RegisterOnGameEvents()
    {
        RegisterOnNodeEncounterEvents();
        GameEvents.DungeonMap.OnFirstSetupDungeon += FirstSetupDungeonListener;
        GameEvents.DungeonMap.OnStartDungeon += StartDungeonListener;
        GameEvents.DungeonMap.OnResumeDungeon += ResumeDungeonListener;
        GameEvents.DungeonMap.OnFinishDungeon += FinishDungeonListener;
    }

    private void UnregisterOnGameEvents()
    {
        GameEvents.DungeonMap.OnFirstSetupDungeon -= FirstSetupDungeonListener;
        GameEvents.DungeonMap.OnStartDungeon -= StartDungeonListener;
        GameEvents.DungeonMap.OnResumeDungeon -= ResumeDungeonListener;
        GameEvents.DungeonMap.OnFinishDungeon -= FinishDungeonListener;
    }

    private void FinishDungeonListener()
    {
        StartCoroutine(FinishDungeon());
        
        IEnumerator FinishDungeon()
        {
            yield return fadeBackground.FadeInRoutine();
            
            yield return Addressables.LoadSceneAsync("City");
        }
    }

    private void ResumeDungeonListener()
        => StartCoroutine(pawn.GoToNextNode());

    private void StartDungeonListener()
        => StartCoroutine(StartDungeon());

    private void FirstSetupDungeonListener()
        => StartCoroutine(SetupDungeon());

    private IEnumerator SetupDungeon()
    {
        pawn.SetPosition(initialPosition.position);
        fadeBackground.SetBackgroundAlpha(1f);
        yield return null;
    }

    private void CacheNodes()
        => dungeonNodes = GameObject
            .FindGameObjectsWithTag("Dungeon node")
            .Select(x => x.GetComponent<DungeonNode>())
            .ToList();

    private void CachePawn()
        => pawn = GameObject
            .FindGameObjectWithTag("Pawn")
            ?.GetComponent<Pawn>();
}