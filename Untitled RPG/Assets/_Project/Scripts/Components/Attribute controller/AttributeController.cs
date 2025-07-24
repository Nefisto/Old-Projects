using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class AttributeController : SerializedMonoBehaviour
{
    [TitleGroup("Debug")]
    [ReadOnly]
    [OdinSerialize]
    private GameAttributes bonusGameAttributes = new();

    [TitleGroup("Debug")]
    [ReadOnly]
    [OdinSerialize]
    private GameAttributes originalGameAttributes = new();

    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    public void Setup (ActorData data)
    {
        bonusGameAttributes = new GameAttributes();
        // originalAttributesProvider = new AttributesProvider(data.TraitChart);
    }

    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    public GameAttributes GetCurrentAttributes() => bonusGameAttributes + originalGameAttributes;
}