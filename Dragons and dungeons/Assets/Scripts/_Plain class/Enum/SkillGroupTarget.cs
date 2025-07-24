/// <summary>
/// This will guide which group this skill should target
///  - Friendly and enemy is based on caster
///     e.g. The player group is friendly if the caster is a player character
///  - This isn't the final word, character will run another logic that can change values
///     e.g. A confused any can cast a FRIENDLY skill in the ENEMY group 
/// </summary>
public enum SkillGroupTarget
{
    Selectable, // Select arbitrary number of targets - For enemies this will be treated as SINGLE
    Single, // One target only
    Myself,
    Friendly, // Actors from the same side, including caster
    FriendlyButMe, // Actors from the same, excluding caster 
    AllEnemies, // Actors from other side
    Everyone, // Target everyone, including caster
    EveryoneButMe // Target everyone, excluding caster
}