public class StatusEffectContext
{
    public BattleActor caster;

    /// <summary>
    ///     On a renew context this is the data that has been detected as repeated
    /// </summary>
    public StatusEffectData overrideData;

    public BattleActor target;
}