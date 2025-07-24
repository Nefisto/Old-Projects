using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Data that persist between runs, like what was/wasn't unlocked
/// </summary>
[CreateAssetMenu(fileName = "Account data", menuName = EditorConstants.MenuAssets.GAME_MENU + "Account data")]
public class AccountData : ScriptableObject
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public int PointsToDistribute { get; private set; } = GameConstants.INITIAL_AMOUNT_OF_ATTRIBUTE_POINTS;

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public int AmountOfUnlockedSlots { get; private set; } = GameConstants.INITIAL_AMOUNT_OF_UNLOCKED_SKILLS_SLOTS;
}