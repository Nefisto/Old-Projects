using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class AttributeView : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text attributeNameLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text attributeLevelLabel;

    [TitleGroup("References")]
    [SerializeField]
    private AttributeLevelBarView attributeLevelBar;

    [TitleGroup("Debug")]
    [DisableInEditorButton]
    public void Setup(Trait trait)
    {
        attributeNameLabel.text = Helper.AttributeTypeToAttributeName(trait.AttributeType);
        attributeLevelLabel.text = $"{trait.CompletedPoints}";
        attributeLevelBar.Setup(trait.PartialPoints);
    }
}