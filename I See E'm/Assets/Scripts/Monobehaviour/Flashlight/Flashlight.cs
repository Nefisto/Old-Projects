using System;
using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Flashlight : LazyBehavior
{
    [TabGroup("Status")]
    [Title("Settings")]
    [SerializeField]
    private float maxAngle = 80;

    [TabGroup("Status")]
    [SerializeField]
    private float maxIntensity = 60f;
    
    private void OnEnable()
    {
        Player.OnUpdateFuel += UpdateLightAngle;
    }

    private void OnDisable()
    {
        Player.OnUpdateFuel -= UpdateLightAngle;
    }

    private void ResetLightAngle()
    {
        light.innerSpotAngle = maxAngle;
    }
    
    [Button]
    private void UpdateLightAngle (float fuelPercentage)
    {
        if (fuelPercentage != 0f)
        {
            light.enabled = true;
        }

        // Convert the percentage to an actual value
        var newAngleValue = maxAngle * fuelPercentage;
        var newIntensity = maxIntensity * fuelPercentage;

        DOTween.To(() => light.innerSpotAngle, x => light.innerSpotAngle = x, newAngleValue, .5f);
        if (fuelPercentage < .35f) // Reduce angle
            DOTween.To(() => light.intensity, x => light.intensity = x, newIntensity, 2f);
    }

    private void DisableFlashlight()
        => light.enabled = false;
}