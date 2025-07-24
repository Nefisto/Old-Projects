#if UNITY_EDITOR
using Sirenix.OdinInspector;

public partial class CoinAnimation
{
    [Button]
    private void RunAnimation()
    {
        StartCoroutine(Animate());
    }
}
#endif