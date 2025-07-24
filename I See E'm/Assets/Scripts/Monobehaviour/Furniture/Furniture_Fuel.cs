using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class Furniture
{
    #region Serialized Field

    [TabGroup("Light", true)]
    [Min(0f)]
    [Title("Status")]
    [SerializeField]
    private float maxFuel = 10f;
        
    [TabGroup("Light")]
    [Min(0f)]
    [Tooltip("Represent the max intensity for light.\n\nIf let in default (0) value it will cache your light ref intensity as initial amount")]
    [SerializeField]
    private float maxIntensity;

    [TabGroup("Light")]
    [Min(0f)]
    [Tooltip("Represent the max range for light.\n\nIf let in default (0) value it will cache your light ref range as initial amount")]
    [SerializeField]
    private float maxLightRange;
    
    [TabGroup("Light")]
    [Min(0f)]
    [Tooltip("Represent the max radius for collision detection.\n\nIf let in default (0) value it will cached your sphere collider radius as initial amount")]
    [SerializeField]
    private float maxDetectionRadius;

    [TabGroup("Light")]
    [Title("Reference")]
    [SerializeField]
    private new Light light;

    [TabGroup("Light")]
    [Tooltip("Time took to tween intensity/radius when change value")]
    [Range(0f, 2f)]
    [Title("Tween")]
    [SerializeField]
    private float tweenDuration = .5f;

    #endregion

    #region Properties

    public bool CanDetectEnemies => light != null && currentFuel > 0f;

    #endregion

    #region Debug fields

    [TabGroup("Light")]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private float currentFuel;

    [TabGroup("Light")]
    [InlineButton("Consume")]
    [DisableInEditorMode]
    [SerializeField]
    #pragma warning disable 414
    private float consumeAmount = 1f;

    [TabGroup("Light")]
    [InlineButton("Refuel")]
    [DisableInEditorMode]
    [SerializeField]
    #pragma warning disable 414
    private float refuelAmount = 5f;
    
    #endregion

    #region API

    public void Consume (int amount)
    {
        currentFuel = Mathf.Clamp(currentFuel - amount, 0, maxFuel);

        UpdateIntensity();
    }

    public void Refuel(int amount)
    {
        currentFuel = Mathf.Clamp(currentFuel + amount, 0, maxFuel);
        
        UpdateIntensity();
    }

    #endregion

    #region Private Methods

    private void UpdateIntensity()
    {
        var fuelPercent = currentFuel / maxFuel;
        var targetIntensity = maxIntensity * fuelPercent;
        var targetLightRange = maxLightRange * fuelPercent;
        var targetRadius = maxDetectionRadius * fuelPercent;

        light.DOIntensity(targetIntensity, tweenDuration);
        DOTween.To(() => light.range, x => light.range = x, targetLightRange, tweenDuration);
        DOTween.To(() => sphereCollider.radius, x => sphereCollider.radius = x, targetRadius, tweenDuration);
    }
    
    private void SetupCollider()
    {
        if (maxDetectionRadius.IsNearlyEnoughTo(0f))
            maxDetectionRadius = sphereCollider.radius;
    }

    private void SetupLight()
    {
        if (maxIntensity.IsNearlyEnoughTo(0f))
            maxIntensity = light.intensity;
        
        if (maxLightRange.IsNearlyEnoughTo(0f))
            maxLightRange = light.range;
        
        currentFuel = maxFuel;
    }

    #endregion
}