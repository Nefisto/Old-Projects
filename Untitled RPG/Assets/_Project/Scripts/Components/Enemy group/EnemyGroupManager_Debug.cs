using Sirenix.OdinInspector;

#if UNITY_EDITOR
public partial class EnemyGroupManager
{
    [TitleGroup("Debug")]
    [ButtonGroup("Debug/EnemyGroups")]
    private void One()
    {
        HideAllGroups();
        GetGroupWith(1).ShowPlaceholder();
    }

    [TitleGroup("Debug")]
    [ButtonGroup("Debug/EnemyGroups")]
    private void Two()
    {
        HideAllGroups();
        GetGroupWith(2).ShowPlaceholder();
    }

    [TitleGroup("Debug")]
    [ButtonGroup("Debug/EnemyGroups")]
    private void Three()
    {
        HideAllGroups();
        GetGroupWith(3).ShowPlaceholder();
    }

    [TitleGroup("Debug")]
    [ButtonGroup("Debug/EnemyGroups")]
    private void Four()
    {
        HideAllGroups();
        GetGroupWith(4).ShowPlaceholder();
    }

    [TitleGroup("Debug")]
    [ButtonGroup("Debug/EnemyGroups")]
    private void Five()
    {
        HideAllGroups();
        GetGroupWith(5).ShowPlaceholder();
    }

    private void HideAllGroups()
    {
        foreach (var (_, group) in enemiesAmountToGroupSettings)
            group.HidePlaceholders();
    }
}
#endif