using UnityEngine;

public class BattleLogMessagePooler : SerializedMonobehaviourPooler<BattleLogMessage>
{
    private void Awake() => ServiceLocator.BattleLogPooler = this;

    private void Start()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }
}