using System.Collections.Generic;
using Sirenix.OdinInspector;

public partial class ActionInfo
{
    [TabGroup("Resistance \\ Proficiency", useFixedHeight: true)]
    [ActionInfoDisplay]
    public float resistanceBonus;

    [TabGroup("Resistance \\ Proficiency")]
    [ActionInfoDisplay]
    public float proficiencyBonus;

    [TabGroup("Resistance \\ Proficiency")]
    [HideReferenceObjectPicker]
    public List<EffectInfo> effectInfo = new();
}