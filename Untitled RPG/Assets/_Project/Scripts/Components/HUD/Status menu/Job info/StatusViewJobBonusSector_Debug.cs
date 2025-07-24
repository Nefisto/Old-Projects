using Sirenix.OdinInspector;

#if UNITY_EDITOR
public partial class StatusViewJobBonusSector
{
    [Button]
    private void T_Fill (int points)
    {
        Clean();
        Fill(points);
    }
}
#endif