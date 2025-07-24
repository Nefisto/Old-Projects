using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayableCharacterData : ActorData
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public CurrentEquipment CurrentEquipment { get; private set; }

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public GameJob Job { get; private set; }

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public List<Skill> SkillSlots { get; private set; } = new();

    [field: TitleGroup("Debug")]
    [field: ReadOnly]
    [field: SerializeField]
    public int CurrentExperience { get; private set; }

    [field: TitleGroup("Debug")]
    [field: ReadOnly]
    [field: ShowInInspector]
    public int CurrentCurrency { get; private set; }

    public override Sprite ATBIcon
    {
        get => Database.GameIcons.PlayerAtbIcon;
        protected set => _ = value;
    }

    public override MainTraitKind MainTraitKind
        => CurrentEquipment?.CurrentWeapon?.MainTraitKind ?? MainTraitKind.Strength;

    /// <summary>
    ///     Potential comes from:
    ///     - Trait [OK]
    ///     - Equipment [OK]
    ///     - Class [OK]
    ///     - Buffs
    /// </summary>
    [ShowInInspector]
    public override int StrengthPotential
        => (TraitChart?.StrengthPotential ?? 0)
           + (CurrentEquipment?.StrengthPotential ?? 0)
           + (Job?.StrengthPotential ?? 0);

    [ShowInInspector]
    public override int VitalityPotential
        => (TraitChart?.VitalityPotential ?? 0)
           + (CurrentEquipment?.VitalityPotential ?? 0)
           + (Job?.VitalityPotential ?? 0);

    [ShowInInspector]
    public override int DexterityPotential
        => (TraitChart?.DexterityPotential ?? 0)
           + (CurrentEquipment?.DexterityPotential ?? 0)
           + (Job?.DexterityPotential ?? 0);

    [ShowInInspector]
    public override int IntelligencePotential
        => (TraitChart?.IntelligencePotential ?? 0)
           + (CurrentEquipment?.IntelligencePotential ?? 0)
           + (Job?.IntelligencePotential ?? 0);

    public override IEnumerator Setup (EntityDataSetupContext ctx)
    {
        yield return base.Setup(ctx);

        var correctSetup = (Context)ctx;
        if (correctSetup.equipment == null)
            CurrentEquipment.Setup();
        else
            CurrentEquipment = correctSetup.equipment;

        if (correctSetup.traitChart != null)
            TraitChart = correctSetup.traitChart;

        ChangeJob(Database.GameJobs.Fallback);

        LevelUpTo(1);
    }

    public void ChangeJob (GameJob to) => Job = to;

    public void AddExperience (int experience)
    {
        CurrentExperience += experience;
        CurrentLevel = Database.GameConstantsSo.ExperienceTable.GetLevelFromExperience(CurrentExperience);
    }

    public void AddCurrency (int currency) => CurrentCurrency += currency;

    public void SetCurrency (int currency) => CurrentCurrency = currency;

    public void RemoveCurrency (int amount)
    {
        if (amount > CurrentCurrency)
            return;

        CurrentCurrency -= amount;
    }

    public class Context : EntityDataSetupContext
    {
        public CurrentEquipment equipment;
        public TraitChart traitChart;
    }
}