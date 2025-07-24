using UnityEngine;

public static class BattleFormulas
{
    public static float GetDamageReduction (int defense, int attackerLevel)
        => (float)defense / (defense + 85 * attackerLevel);

    public static float GetEvadeBonus (int evasion, int attackerLevel)
        => Mathf.Min((float)evasion / (evasion + (attackerLevel * 12 - evasion)), 2f);

    public static float GetHitBonus (int accuracy, int attackerLevel)
        => Mathf.Min((float)accuracy / (accuracy + (attackerLevel * 12 - accuracy)), 2f);

    public static float GetSkillChanceToHit (float skillBaseChanceToHit, float hitBonus, float evasionBonus)
        => Mathf.Clamp(skillBaseChanceToHit * (1 + (hitBonus - evasionBonus)), .05f, .95f);

    public static float GetCriticalChance (float baseChance, int criticalAttribute)
        => Mathf.Clamp(baseChance * (1 + criticalAttribute / 100f), .05f, .95f);

    public static int ApplyCriticalDamage (int damage, int criticalDamageAttribute)
        => (int)(damage * (1 + criticalDamageAttribute / 100f));

    public static float GetResistanceBonus (int resistance, int attackerLevel)
        => Mathf.Min((float)resistance / (resistance + (attackerLevel * 12 - resistance)), 2f);

    public static float GetProficiencyBonus (int proficiency, int attackerLevel)
        => Mathf.Min((float)proficiency / (proficiency + (attackerLevel * 12 - proficiency)), 2f);

    public static float GetEffectChanceToApply (float effectBaseChanceToApply, float proficiencyBonus,
        float resistanceBonus)
        => Mathf.Clamp(effectBaseChanceToApply * (1 + (proficiencyBonus - resistanceBonus)), .05f, .95f);
}