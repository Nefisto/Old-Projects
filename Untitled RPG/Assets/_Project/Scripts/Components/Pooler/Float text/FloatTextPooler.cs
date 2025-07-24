using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class FloatTextPooler : SerializedMonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private List<TMP_Text> pool;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text textPrefab;

    public TMP_Text GetFloatText ()
    {
        var foundTMPText = pool
            .FirstOrDefault(t => t.gameObject.activeInHierarchy == false);

        if (foundTMPText == null)
            IncreasePool();

        foundTMPText = pool
            .FirstOrDefault(t => t.gameObject.activeInHierarchy == false);

        return foundTMPText;
    }

    private void IncreasePool()
    {
        var amountToIncrease = Mathf.Max((int)(pool.Count * .5f), 10);
        for (var i = 0; i < amountToIncrease; i++)
        {
            var instance = Instantiate(textPrefab, transform, false);
            instance.gameObject.SetActive(false);

            pool.Add(instance);
        }
    }
}