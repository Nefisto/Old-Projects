using Sirenix.OdinInspector;
using UnityEngine;

public partial class TraitChart
{
    [TitleGroup("Debug")]
    [OnValueChanged(nameof(LevelUpTo))]
    [Range(1, 100)]
    [ShowInInspector]
    private int level = 1;
}