using System.Collections.Generic;
using Sirenix.OdinInspector;

public partial class ActionInfo
{
    [TabGroup("Critical", useFixedHeight: true)]
    [ActionInfoDisplay]
    public bool hadCritical;

    [TabGroup("Critical")]
    [ActionInfoDisplay]
    public int criticalAttribute;

    [TabGroup("Critical")]
    [ActionInfoDisplay]
    public float baseCritical;

    [TabGroup("Critical")]
    [ActionInfoDisplay]
    [ShowInInspector]
    public List<(string, float)> criticalBonus = new();

    [TabGroup("Critical")]
    [ActionInfoDisplay]
    public float criticalChance;

    [TabGroup("Critical")]
    [ActionInfoDisplay]
    public float criticalDamagePercentage;
}