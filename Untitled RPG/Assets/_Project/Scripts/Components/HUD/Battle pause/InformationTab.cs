using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class InformationTab : MonoBehaviour
{
    [FormerlySerializedAs("entryPrefab")]
    [TitleGroup("References")]
    [SerializeField]
    private AttributeInfoOnPauseEntry onPauseEntryPrefab;

    [TitleGroup("References")]
    [SerializeField]
    private Transform leftColumn;

    [TitleGroup("References")]
    [SerializeField]
    private Transform rightColumn;

    private readonly Dictionary<string, AttributeInfoOnPauseEntry> attributeNameToEntry = new();

    public void Setup (PlayableCharacterData characterData)
    {
        CleanColumns();
        attributeNameToEntry.Clear();

        var amountOfAttributes = characterData.GameAttributesEnumerator().Count();
        var addedAlreadyCount = 0;
        foreach (var (attributeName, attributeValue) in characterData.GameAttributesEnumerator())
        {
            var correctColumn = addedAlreadyCount <= amountOfAttributes * .5f
                ? leftColumn
                : rightColumn;

            var entry = Instantiate(onPauseEntryPrefab, correctColumn.transform, false);
            entry.Setup(attributeName, attributeValue);

            attributeNameToEntry.Add(attributeName, entry);

            addedAlreadyCount++;
        }
    }

    public void Refresh (PlayableCharacterData characterData)
    {
        foreach (var (attributeName, attributeValue) in characterData.GameAttributesEnumerator())
            attributeNameToEntry[attributeName].Setup(attributeName, attributeValue);
    }

    private void CleanColumns()
    {
        foreach (Transform child in leftColumn.transform)
            Destroy(child.gameObject);

        foreach (Transform child in rightColumn.transform)
            Destroy(child.gameObject);
    }
}