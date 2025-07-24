using UnityEngine;

public class DamageActionResult : ForeseeActionResult
{
    [Range(0f, 1f)]
    public float chanceToHit;

    public bool willKill;
    public bool willTakeDamage;

    public override int Priority
    {
        get
        {
            var willKillPoints = willKill ? ActionPriorityUpdate.PositiveMedium : ActionPriorityUpdate.PositiveSmall;
            var canBlockDamagePoints = willTakeDamage ? ActionPriorityUpdate.PositiveMedium : ActionPriorityUpdate.Zero;

            return (int)(((int)willKillPoints + (int)canBlockDamagePoints) * chanceToHit);
        }
    }
}