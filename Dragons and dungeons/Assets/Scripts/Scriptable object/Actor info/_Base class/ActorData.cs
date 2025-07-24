using System;
using System.Collections.Generic;
using System.Linq;
using Ludiq.PeekCore;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Base class for all our battle actors
/// </summary>
public abstract partial class ActorData : SerializedScriptableObject, ICloneable
{
    [TitleGroup("Basic")]
    [HorizontalGroup("Basic/split", .3f)]
    [PreviewField(ObjectFieldAlignment.Left)]
    [HideLabel]
    public Sprite icon;

    [VerticalGroup("Basic/split/data")]
    [LabelWidth(50f)]
    public new string name;

    [Space]
    [TabGroup("Data tab", "Status")]
    [PropertyOrder(10)]
    [SerializeField]
    private List<Skill> defaultSkills;
    
    public virtual int Level { get; }

    protected virtual ActorData CreateNewInstance => CreateInstance<ActorData>();

    public Status GetCurrentStatus()
        => GetBaseStatus() + GetPassiveStatus();
    
    public abstract Status GetBaseStatus();

    public virtual Status GetPassiveStatus()
    {
        var temporaryStatus = Status.EmptyStatus;
        foreach (var skillPassive in GetPassiveSkill())
        {
            if (skillPassive is IStatusChange sc)
            {
                temporaryStatus += sc.ChangeStatus(new StatusChangeContext(){baseStatus = GetBaseStatus()});
            }
        }

        return temporaryStatus;
    }

    protected IEnumerable<SkillPassive> GetPassiveSkill()
    {
        foreach (var weaponPassiveSkill in GetWeaponPassiveSkills())
            yield return weaponPassiveSkill;
        
        // Follow later with other equipment passive

        // Then mastery passive

        // Then a skill tree passive
    }

    // Temporary Data
    private int TemporaryStrength = 0;
    private int TemporaryDexterity = 0;
    private int TemporaryIntelligence = 0;

    public void AddTemporaryStatus (StatusEnum statusEnum, int amount)
    {
        switch (statusEnum)
        {
            case StatusEnum.Strength:
                TemporaryStrength += amount;
                break;

            case StatusEnum.Dexterity:
                TemporaryDexterity += amount;
                break;

            case StatusEnum.Intelligence:
                TemporaryIntelligence += amount;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(statusEnum), statusEnum, null);
        }
    }

    public virtual object Clone()
    {
        var clone = (ActorData)CreateNewInstance.MemberwiseClone();

        clone.icon = icon;
        clone.name = string.Copy(name);
        clone.defaultSkills = defaultSkills.ToList();
        clone.inventory = inventory != null ? (InventoryData)inventory.Clone() : null; 
        
        EquipmentClone(clone);

        return clone;
    }
}