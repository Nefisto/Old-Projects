using Sirenix.OdinInspector;
using UnityEngine;

public partial class CoinPooler : MonoBehaviour, ICoinPooler
{
    [TitleGroup("Settings")]
    [SerializeField]
    private CoinAnimation coinPrefab;

    private void Awake()
    {
        ServiceLocator.CoinPooler = this;
    }
}