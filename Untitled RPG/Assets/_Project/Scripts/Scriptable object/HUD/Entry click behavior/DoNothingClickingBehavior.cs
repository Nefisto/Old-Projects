using UnityEngine;

[CreateAssetMenu(fileName = "Do nothing", menuName = EditorConstants.MenuAssets.CLICK_BEHAVIOR + "Do nothing",
    order = -5)]
public class DoNothingClickingBehavior : ClickBehavior
{
    protected override void Behavior (ClickBehaviorContext ctx) { }
}