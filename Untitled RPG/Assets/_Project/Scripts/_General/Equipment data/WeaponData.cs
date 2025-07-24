using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public interface IPassive
{
    public IEnumerable<PassiveSkill> GetPassives();
}

[Serializable]
public class WeaponData : EquipmentData, IPassive
{
    public enum WeaponSpeed
    {
        Slow,
        Normal,
        Fast
    }

    [HorizontalGroup("Settings/Header")]
    [VerticalGroup("Settings/Header/Right")]
    [PropertyOrder(1)]
    [SerializeField]
    private WeaponSpeed gearSpeed = WeaponSpeed.Normal;

    [field: TitleGroup("Trait")]
    [field: SerializeField]
    public MainTraitKind MainTraitKind { get; protected set; }

    [TitleGroup("Skills", "Passive")]
    [SerializeField]
    private List<PassiveSkill> passiveSkills;

    public int WeaponATBPoints
        => gearSpeed switch
        {
            WeaponSpeed.Slow => 1,
            WeaponSpeed.Normal => 2,
            WeaponSpeed.Fast => 4,
            _ => throw new ArgumentOutOfRangeException()
        };

    protected override string PotentialLabel
    {
        get => Potential.NicefiedStringWithMain(MainTraitKind);
        set => _ = value;
    }

    public override EquipmentKind EquipmentKind => EquipmentKind.Weapon;

    public IEnumerable<PassiveSkill> GetPassives()
        => passiveSkills
            .Select(ps => ps.GetInstance);
}