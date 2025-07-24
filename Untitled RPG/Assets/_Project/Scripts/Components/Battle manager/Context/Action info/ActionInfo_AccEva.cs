using System.Collections.Generic;
using Sirenix.OdinInspector;

public partial class ActionInfo
{
    [TabGroup("Hit \\ Evade")]
    [ActionInfoDisplay]
    public int evasionAttribute;

    [TabGroup("Hit \\ Evade")]
    [ActionInfoDisplay]
    public float evasionBonus;

    [TabGroup("Hit \\ Evade")]
    [ActionInfoDisplay]
    public int accuracyAttribute;

    [TabGroup("Hit \\ Evade")]
    [ActionInfoDisplay]
    public float hitBonus;

    [TabGroup("Hit \\ Evade")]
    [ActionInfoDisplay]
    public float chanceToHit;

    [TabGroup("Hit \\ Evade")]
    [ActionInfoDisplay]
    [ShowInInspector]
    public List<(string, float)> flatBonusToHit = new();

    [TabGroup("Hit \\ Evade", useFixedHeight: true)]
    [ActionInfoDisplay]
    public bool hasMissed;
}