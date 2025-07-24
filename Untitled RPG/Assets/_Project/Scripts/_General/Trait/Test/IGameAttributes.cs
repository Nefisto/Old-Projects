using System.Collections.Generic;

public interface IGameAttributes
{
    public int Damage { get; }

    public int StrengthDamage { get; }
    public int DexterityDamage { get; }
    public int VitalityDamage { get; }
    public int IntelligenceDamage { get; }

    // STR
    public int HealthRegen { get; }
    public int Accuracy { get; }
    public int CriticalDamage { get; }

    // VIT
    public int HealthMax { get; }
    public int Defense { get; }
    public int Resistance { get; }

    // DEX
    public int Evasion { get; }
    public int CriticalChance { get; }
    public int AttackSpeed { get; }

    // INT
    public int Proficiency { get; }
    public int ManaMax { get; }
    public int ManaRegen { get; }

    // Other
    public int RecoveryReduction { get; }

    public IEnumerable<(string attributeName, int attributeValue)> GameAttributeEnumerator()
    {
        yield return ("Damage", Damage);
        yield return (nameof(HealthMax), HealthMax);
        yield return (nameof(Proficiency), Proficiency);
        yield return (nameof(HealthRegen), HealthRegen);
        yield return (nameof(Defense), Defense);
        yield return (nameof(Resistance), Resistance);
        yield return (nameof(RecoveryReduction), RecoveryReduction);
        yield return (nameof(Accuracy), Accuracy);
        yield return (nameof(CriticalChance), CriticalChance);
        yield return (nameof(CriticalDamage), CriticalDamage);
        yield return (nameof(AttackSpeed), AttackSpeed);
        yield return (nameof(Evasion), Evasion);
        yield return (nameof(ManaMax), ManaMax);
        yield return (nameof(ManaRegen), ManaRegen);
    }
}