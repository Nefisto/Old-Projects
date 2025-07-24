using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Timers;
using NTools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Detector
{
    public GameObject gameObject;
    public bool canScare;

    public Detector() { }

    public Detector (GameObject detector, bool canScare)
    {
        this.gameObject = detector;
        this.canScare = canScare;
    }
}

public partial class Enemy
{
    /// <summary>
    /// After being fully detected
    /// </summary>
    public Action OnDetected;

    /// <summary>
    /// Some detector did not scare, light furniture for example, it reveal but does not scare
    /// </summary>
    public bool IsScared => detectedBy.Any(detector => detector.canScare);

    /// <summary>
    /// Is something revealing this enemy? Use to update the meter in each frame, cause if they are not being detect the meter should go down
    /// </summary>
    public bool IsDetected => detectedBy.Any();

    [TabGroup("Detected")]
    [Title("Detect")]
    [SerializeField]
    private float maxDetectMeter = 100;

    [TabGroup("Detected")]
    [SerializeField]
    private float detectGainRate = 15f;

    [TabGroup("Detected")]
    [SerializeField]
    private float detectLossRate = 10f;

    [TabGroup("Detected")]
    [Tooltip("After got totally uncovered, how long this state will be kept?")]
    [SerializeField]
    private float timeBeforeFadeAgain = 4f;
    
    [TabGroup("Detected")]
    [Tooltip("Is the enemy visible at moment? This will decide if hits will be applied to life or scare meter")]
    [ReadOnly]
    [SerializeField]
    private bool isVisible;

    [TabGroup("Detected")]
    [Title("Flee")]
    [SerializeField]
    private float maxFleeMeter = 45;

    [TabGroup("Detected")]
    [SerializeField]
    private float fleeThreshold = 30f;

    [TabGroup("Detected")]
    [SerializeField]
    private float fleeMeterGainRate = 1;

    [TabGroup("Detected")]
    [SerializeField]
    private float loseFleeMeterRate = 1f;

    #region Debug

    [TabGroup("Detected")]
    [Title("Debug")]
    [SerializeField]
    private GameObject detectMeter;

    [TabGroup("Detected")]
    [ReadOnly]
    [SerializeField]
    private float currentDetectMeter = 0;
    
    [TabGroup("Detected")]
    [Tooltip("This will show the amount of time that this enemy is visible without being noticed by no one")]
    [ReadOnly]
    [SerializeField]
    private float detectedTimer;


    [TabGroup("Detected")]
    [ReadOnly]
    [SerializeField]
    private float currentFleeMeter;

    [TabGroup("Detected")]
    [ReadOnly]
    [SerializeField]
    private List<Detector> detectedBy = new List<Detector>();

    #endregion

    private float detectMeterInitialSize = 0f;
    private Task detectTask;
    
    
    private void SetupDetect()
    {
        detectMeterInitialSize = detectMeter.transform.localScale.x;

        // Make it visible when die
        OnEnemyDie += () => UpdateAlpha(1f);
    }

    public Transform GetNearScareDetectorTransform()
    {
        // First for now
        return detectedBy
            .FirstOrDefault(detector => detector.canScare)
            ?.gameObject
            .transform;
    }

    public void AddDetector (Detector detector)
    {
        if (!detectedBy.Contains(detector))
        {
            detectedBy.Add(detector);
        }
    }

    public void RemoveDetector (GameObject detector)
    {
        detectedBy.RemoveAll(det => det.gameObject == detector);
    }
    
    // Update by hits
    private void UpdateDetect (float? amount = null)
    {
        if (IsDied) // SHAME ON ME
            return;
        
        // Already visible
        if (isVisible)
        {
            if (IsDetected) // If I would gain detect meter this frame, reset the timer
                detectedTimer = 0f;
            
            return;
        }

        // If didn't specify an amount will update based on gain lose rate
        if (!amount.HasValue)
        {
            amount = IsDetected ? detectGainRate : -detectLossRate;
            amount *= Time.deltaTime;
        }
        
        currentDetectMeter = Mathf.Clamp(currentDetectMeter + amount.Value, 0, maxDetectMeter);

        var detectMeterPercentage = currentDetectMeter / maxDetectMeter;
        ResizeDetectMeter(detectMeterPercentage);
        UpdateAlpha(detectMeterPercentage);

        if (detectMeterPercentage.IsNearlyEnoughTo(1f))
        {
            detectTask = new Task(Detected());
        }
    }

    private IEnumerator Detected()
    {
        detectedTimer = 0f;
        isVisible = true;

        while (detectedTimer <= timeBeforeFadeAgain)
        {
            
            detectedTimer += Time.deltaTime;
            yield return null;
        }

        isVisible = false;
    }
    
    private void UpdateFleeMeter()
    {
        if (IsDied) // SHAME ON ME
            return;
        
        var multiplier = IsScared ? fleeMeterGainRate : -loseFleeMeterRate;

        currentFleeMeter = Mathf.Clamp(currentFleeMeter + Time.deltaTime * multiplier, 0f, maxFleeMeter);
    }

    private void CheckFlee()
    {
        if (IsDied) // SHAME ON ME
            return;
        
        if (currentFleeMeter >= fleeThreshold && currentEnemyState != EnemyStates.Pause)
        {
            ChangeState(EnemyStates.Flee);
        }
    }

    private void ResizeDetectMeter (float percentage)
    {
        var newSize = detectMeterInitialSize * percentage;

        var scale = detectMeter.transform.localScale;
        detectMeter.transform.localScale = new Vector3(newSize, scale.y, scale.z);
    }

    private void UpdateAlpha(float percentage)
    {
        foreach (var material in renderers.Select(r => r.material))
        {
            var originalColor = material.color;
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, percentage);
        }

        // Used particles change color based on a different parameter
        foreach (var material in particles.Select(p => p.material))
        {
            var originalColor = material.GetColor(TintColor);
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, percentage);
        }
    }
}