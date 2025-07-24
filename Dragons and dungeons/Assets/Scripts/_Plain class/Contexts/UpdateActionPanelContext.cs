using System.Collections.Generic;

/// <summary>
/// Character will fill it and pass to action panel
/// Action panel will receive it, instantiate the buttons and transfer data to the buttons
/// Buttons will retrieve images and draw on screen
/// </summary>
public class UpdateActionPanelContext
{
    // ActionPanel owner
    public FriendlyBattleActor actor;
    
    // Buffs to show

    // Skill to show
    public IEnumerable<Skill> skills;

    public UpdateActionPanelContext(FriendlyBattleActor actor, IEnumerable<Skill> skills)
    {
        this.actor = actor;
        this.skills = skills;
    }
}