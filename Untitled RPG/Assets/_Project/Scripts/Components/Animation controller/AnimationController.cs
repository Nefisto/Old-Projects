using Sirenix.OdinInspector;
using UnityEngine;

public partial class AnimationController : SerializedMonoBehaviour
{
    [field: TitleGroup("References")]
    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }

    private enum KindOfAnimation
    {
        Normal,
        TakeDamage,
        BecomePoisoned
    }
}