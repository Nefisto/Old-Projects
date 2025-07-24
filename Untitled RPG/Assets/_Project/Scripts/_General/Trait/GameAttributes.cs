using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameAttributes : IEnumerable<(string attributeName, int attributeValue)>
{
    [field: SerializeField]
    public int PhysicalDamage { get; protected set; } = 1;

    [field: SerializeField]
    public int SpecialDamage { get; protected set; } = 1;

    [field: SerializeField]
    public int EffectProficiency { get; protected set; } = 1;

    [field: SerializeField]
    public int HealthMax { get; protected set; } = 1;

    [field: SerializeField]
    public int HealthRegen { get; protected set; } = 1;

    [field: SerializeField]
    public int PhysicalDefense { get; protected set; } = 1;

    [field: SerializeField]
    public int SpecialDefense { get; protected set; } = 1;

    [field: SerializeField]
    public int CriticalDefense { get; protected set; } = 1;

    [field: SerializeField]
    public int Resistance { get; protected set; } = 1;

    [field: SerializeField]
    public int RecoveryReduction { get; protected set; } = 1;

    [field: SerializeField]
    public int Accuracy { get; protected set; } = 1;

    [field: SerializeField]
    public int CriticalChance { get; protected set; } = 1;

    [field: SerializeField]
    public int CriticalDamage { get; protected set; } = 1;

    [field: SerializeField]
    public int Evasion { get; protected set; } = 1;

    [field: SerializeField]
    public int AttackSpeed { get; protected set; } = 1;

    [field: SerializeField]
    public int ManaMax { get; protected set; } = 1;

    [field: SerializeField]
    public int ManaRegen { get; protected set; } = 1;

    [field: SerializeField]
    public int MagicDefense { get; protected set; } = 1;

    [field: SerializeField]
    public int Knowledge { get; protected set; } = 1;

    [field: SerializeField]
    public int Luck { get; protected set; } = 1;

    [field: SerializeField]
    public int Rage { get; protected set; } = 1;

    public IEnumerator<(string attributeName, int attributeValue)> GetEnumerator()
    {
        yield return (nameof(PhysicalDamage), PhysicalDamage);
        yield return (nameof(SpecialDamage), SpecialDamage);
        yield return (nameof(EffectProficiency), EffectProficiency);
        yield return (nameof(HealthMax), HealthMax);
        yield return (nameof(HealthRegen), HealthRegen);
        yield return (nameof(PhysicalDefense), PhysicalDefense);
        yield return (nameof(SpecialDefense), SpecialDefense);
        yield return (nameof(CriticalDefense), CriticalDefense);
        yield return (nameof(Resistance), Resistance);
        yield return (nameof(RecoveryReduction), RecoveryReduction);
        yield return (nameof(Accuracy), Accuracy);
        yield return (nameof(CriticalChance), CriticalChance);
        yield return (nameof(CriticalDamage), CriticalDamage);
        yield return (nameof(AttackSpeed), AttackSpeed);
        yield return (nameof(Evasion), Evasion);
        yield return (nameof(ManaMax), ManaMax);
        yield return (nameof(ManaRegen), ManaRegen);
        yield return (nameof(MagicDefense), MagicDefense);
        yield return (nameof(Knowledge), Knowledge);
        yield return (nameof(Luck), Luck);
        yield return (nameof(Rage), Rage);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static GameAttributes operator + (GameAttributes r, GameAttributes l)
    {
        return new GameAttributes
        {
            PhysicalDamage = r.PhysicalDamage + l.PhysicalDamage,
            SpecialDamage = r.SpecialDamage + l.SpecialDamage,
            EffectProficiency = r.EffectProficiency + l.EffectProficiency,
            HealthMax = r.HealthMax + l.HealthMax,
            HealthRegen = r.HealthRegen + l.HealthRegen,
            PhysicalDefense = r.PhysicalDefense + l.PhysicalDefense,
            SpecialDefense = r.SpecialDefense + l.SpecialDefense,
            CriticalDefense = r.CriticalDefense + l.CriticalDefense,
            Resistance = r.Resistance + l.Resistance,
            RecoveryReduction = r.RecoveryReduction + l.RecoveryReduction,
            Accuracy = r.Accuracy + l.Accuracy,
            CriticalChance = r.CriticalChance + l.CriticalChance,
            CriticalDamage = r.CriticalDamage + l.CriticalDamage,
            AttackSpeed = r.AttackSpeed + l.AttackSpeed,
            Evasion = r.Evasion + l.Evasion,
            ManaMax = r.ManaMax + l.ManaMax,
            ManaRegen = r.ManaRegen + l.ManaRegen,
            MagicDefense = r.MagicDefense + l.MagicDefense,
            Knowledge = r.Knowledge + l.Knowledge,
            Luck = r.Luck + l.Luck,
            Rage = r.Rage + l.Rage
        };
    }
}