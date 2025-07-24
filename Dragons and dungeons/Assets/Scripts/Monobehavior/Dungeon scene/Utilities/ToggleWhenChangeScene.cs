using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ToggleWhenChangeScene : MonoBehaviour
{
    [Title("Control")]
    [SerializeField]
    private List<GameObject> objectInSceneToToggle;

    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private DungeonManager dungeonManager;

    private void Awake()
    {
        CacheDungeonManager();
    }

    private void OnEnable()
    {
        if (dungeonManager)
            RegisterOnDungeonManagerEvents();   
    }

    private void OnDisable()
    {
        if (dungeonManager)
            UnregisterOnDungeonManagerEvents();
    }

    private void RegisterOnDungeonManagerEvents()
    {
        dungeonManager.OnChangeToBattleScene += DisableObjects;

        GameEvents.DungeonMap.OnSetupToResumeDungeon += EnableObjects;
    }

    private void UnregisterOnDungeonManagerEvents()
    {
        dungeonManager.OnChangeToBattleScene -= DisableObjects;

        GameEvents.DungeonMap.OnSetupToResumeDungeon -= EnableObjects;
    }

    private void EnableObjects()
    {
        for (var i = 0; i < objectInSceneToToggle.Count; i++)
        {
            if (objectInSceneToToggle[i] == null)
                throw new Exception($"index {i} is null");

            objectInSceneToToggle[i].SetActive(true);
        }
    }

    private void DisableObjects()
    {
        for (var i = 0; i < objectInSceneToToggle.Count; i++)
        {
            if (objectInSceneToToggle[i] == null)
                throw new Exception($"index {i} is null");

            objectInSceneToToggle[i].SetActive(false);
        }
    }

    private void CacheDungeonManager()
    {
        dungeonManager = GameObject.FindGameObjectWithTag("Dungeon manager")?.GetComponent<DungeonManager>();

        if (dungeonManager == null)
            Debug.LogError("Did not found Dungeon manager to register toggle events", this);
    }
}