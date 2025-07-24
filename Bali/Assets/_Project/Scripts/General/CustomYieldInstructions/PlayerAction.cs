using System.Collections;
using UnityEngine;

public class PlayerAction : CustomYieldInstruction
{
    private readonly Player player;

    public PlayerAction (Player player)
        => this.player = player;

    public override bool keepWaiting { get; }
    // public Skill

    // public IEnumerator Run()
    // {
    //     yield return player.Act();
    // }
}