using Sirenix.OdinInspector;
using UnityEngine;

public class AlphaLevel : MonoBehaviour
{
    [Title("Settings")]
    [Tooltip("Alpha when not selected")]
    public float notSelectedAlpha = .2f;

    [Tooltip("Alpha when selected")]
    public float selectedAlpha = 1f;
}