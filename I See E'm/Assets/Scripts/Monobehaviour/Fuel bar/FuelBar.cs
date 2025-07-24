using System;
using DG.Tweening;
using NTools;
using UnityEngine;

public class FuelBar : LazyBehavior
{
    [SerializeField]
    private Gradient gradient;
        
    private void OnEnable()
    {
        Player.OnUpdateFuel += UpdateFuelBar;
    }

    private void OnDisable()
    {
        Player.OnUpdateFuel -= UpdateFuelBar;
    }

    private void UpdateFuelBar(float newPercentage)
    {
        image.color = gradient.Evaluate(newPercentage);
        
        image.fillAmount = newPercentage;
    }
}