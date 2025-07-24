using Sirenix.OdinInspector;

public abstract class LocationModifier : PassiveSkill
{
    [ShowInInspector]
    public abstract string NameShowOnField { get; }
}