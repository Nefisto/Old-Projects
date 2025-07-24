using Sirenix.OdinInspector;
using UnityEngine;

public partial class Enemy
{
    [TabGroup("Debug", true, -5f)]
    [Title("Settings")]
    [SerializeField]
    private bool startInvisible = true;
    
    [TabGroup("Debug")]
    [SerializeField]
    private bool doAlphaTransition;

    [TabGroup("Debug")]
    [SerializeField]
    private GizmosDebug gizmosDebug = GizmosDebug.None;
}