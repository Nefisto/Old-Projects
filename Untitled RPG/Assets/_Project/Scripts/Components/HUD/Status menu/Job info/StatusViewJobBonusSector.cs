using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class StatusViewJobBonusSector : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private List<Image> points;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text counter;

    public IEnumerator Setup (int bonus)
    {
        Clean();
        Fill(bonus);
        yield break;
    }

    private void Fill (int bonus)
    {
        foreach (var point in points.Take(bonus))
            point.fillCenter = true;

        counter.text = $"{bonus} / 8";
    }

    private void Clean()
    {
        foreach (var point in points)
            point.fillCenter = false;

        counter.text = "0 / 8";
    }
}