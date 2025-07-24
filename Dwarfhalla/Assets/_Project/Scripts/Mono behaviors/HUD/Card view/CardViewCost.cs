using System;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CardViewCost : MonoBehaviour
{
    [TitleGroup("Debug")]
    [SerializeField]
    private List<Transform> actionPoints;

    private List<Image> apIcons;
    
    public void SetPoints (int points)
    {
        actionPoints.ForEach(child => child.gameObject.SetActive(false));
        
        actionPoints
            .Take(points)
            .ForEach(child => child.gameObject.SetActive(true));
        
        apIcons = actionPoints.Select(ap => ap.GetComponentInChildren<Image>()).ToList();
    }
    
    public void SetAPIconsAlpha(float alpha) => apIcons.ForEach(ap => ap.color = ap.color.SetAlpha(alpha));
}