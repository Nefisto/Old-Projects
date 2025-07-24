using Sirenix.OdinInspector;
using UnityEngine;

public partial class GameConstantsSO
{
    [field: TitleGroup("Skills", "Coin throw")]
    [field: Tooltip("How many coins represent 1% of damage increase in coin throw skill")]
    [field: Min(1)]
    [field: SerializeField]
    public int AmountOfCoinsPerOnePercentIncreaseInCoinThrow { get; private set; } = 7;

    [field: TitleGroup("Skills", "Poison")]
    [field: Min(1)]
    [field: SerializeField]
    public int MaxAmountOfPoisonStacks { get; private set; } = 5;
}