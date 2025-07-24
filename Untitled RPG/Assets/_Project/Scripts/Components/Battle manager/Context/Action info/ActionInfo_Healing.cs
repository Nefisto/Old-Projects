using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class ActionInfo
{
    [TabGroup("Healing")]
    [ActionInfoDisplay]
    public int flatHeal;

    [TabGroup("Healing")]
    [ShowInInspector]
    public List<(string info, float percentage)> percentageOfFlatHealBonus = new();

    [TabGroup("Healing")]
    [ShowInInspector]
    public int FinalHealing => Mathf.CeilToInt(flatHeal * (1 + percentageOfFlatHealBonus.Sum(t => t.percentage)));
}