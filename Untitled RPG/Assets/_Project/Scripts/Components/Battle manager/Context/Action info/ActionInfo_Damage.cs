using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class ActionInfo
{
    [TabGroup("Damage", useFixedHeight: true)]
    [ActionInfoDisplay]
    public int flatDamage;

    [TabGroup("Damage")]
    [ActionInfoDisplay]
    public int multipliedDamage;

    [TabGroup("Damage")]
    [ActionInfoDisplay]
    [ShowInInspector]
    public List<(string info, float percentage)> percentageOfFlatDamageBonus = new();

    [TabGroup("Damage")]
    [ActionInfoDisplay]
    public int damageAfterCritical;

    [TabGroup("Damage")]
    [HideReferenceObjectPicker]
    public List<DamageModifierPassiveInfo> multiplyBaseDamage = new();

    [TabGroup("Damage")]
    [HideReferenceObjectPicker]
    public List<DamageModifierPassiveInfo> flatExtraDamage = new();

    [TabGroup("Damage")]
    [ActionInfoDisplay]
    [Tooltip("Caster damage IF not critical\nCritical damage otherwise")]
    [ShowInInspector]
    public int BaseDamage
    {
        get
        {
            return Mathf.RoundToInt(
                ActualDamageToCountAsBase
                + flatExtraDamage.Sum(entry => entry.value));
        }
    }

    [TabGroup("Damage")]
    [ActionInfoDisplay]
    [ShowInInspector]
    public int FinalDamage => BaseDamage - mitigatedDamage;

    private int ActualDamageToCountAsBase => hadCritical ? damageAfterCritical : multipliedDamage;
}