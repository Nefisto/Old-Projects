using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

public partial class EnemyGroupManager : SerializedMonoBehaviour
{
    [Title("Settings")]
    [SerializeField]
    private NDictionary<int, EnemyGroupSettings> enemiesAmountToGroupSettings;

    private void Start()
    {
        foreach (var (_, group) in enemiesAmountToGroupSettings)
            group.HidePlaceholders();
    }

    public void DestroyAllGroups()
    {
        foreach (var (_, group) in enemiesAmountToGroupSettings)
            group.DestroyEnemies();
    }

    public EnemyGroupSettings GetGroupWith (int amountOfEnemies)
    {
        Assert.IsTrue(enemiesAmountToGroupSettings.ContainsKey(amountOfEnemies));

        return enemiesAmountToGroupSettings[amountOfEnemies];
    }
}