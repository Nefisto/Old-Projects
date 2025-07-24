using System;
using UnityEngine;

[Serializable]
public class FloatTextSettings
{
    public string message;

    public Transform targetTransform;

    public float timeOnScreen;
    public float upSpeed;

    /// <summary>
    ///     Lower number happens first
    ///     Default: 0
    /// </summary>
    public int priority;

    public Color textColor = Color.white;

    public FloatTextSettings (string message, Transform targetTransform, float timeOnScreen = 1f, float upSpeed = 1f, Color? textColor = null)
    {
        this.message = message;
        this.targetTransform = targetTransform;
        this.timeOnScreen = timeOnScreen;
        this.upSpeed = upSpeed;
        this.textColor = textColor ?? Color.white;
    }
}