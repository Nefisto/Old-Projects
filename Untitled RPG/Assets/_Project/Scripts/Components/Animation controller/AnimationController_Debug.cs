using System;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
public partial class AnimationController
{
    [Button]
    [DisableInEditorMode]
    private void RunAnimation (KindOfAnimation kind)
    {
        switch (kind)
        {
            case KindOfAnimation.TakeDamage:
                StartCoroutine(TakeDamageAnimation());
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }
    }
}
#endif