using Sirenix.OdinInspector;

public partial class ActionInfo
{
    [TabGroup("Defense", useFixedHeight: true)]
    [ActionInfoDisplay]
    public int targetDefense;

    [TabGroup("Defense")]
    [ActionInfoDisplay]
    public float mitigatedPercentage;

    [TabGroup("Defense")]
    [ActionInfoDisplay]
    public int mitigatedDamage;
}