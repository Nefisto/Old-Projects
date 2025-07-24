using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public partial class Enemy : LazyBehavior, IDamageable
{
    [TabGroup("Basic")]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private List<Renderer> renderers;

    [TabGroup("Basic")]
    [ReadOnly]
    [SerializeField]
    private List<Renderer> particles; // Instead of color, this particle use _TintColor

    private static readonly int TintColor = Shader.PropertyToID("_TintColor");

    #region Monobehaviour callbacks

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>().ToList();
        for (var i = renderers.Count - 1; i >= 0; i--)
        {
            if (renderers[i].TryGetComponent<ParticleSystem>(out _))
            {
                particles.Add(renderers[i]);
                renderers.RemoveAt(i);
            }
        }
        
        SetupDetect();
        SetupLife();
    }

    private void Start()
    {
        SetupBehaviorMachine();
    }

    private void Update()
    {
        behaviorMachine.CurrentState.LogicUpdate();
        
        UpdateFleeMeter();
        CheckFlee();

        if (tookDamageThisTurn)
        {
            tookDamageThisTurn = false;
            return;
        }
        
        UpdateDetect();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (gizmosDebug == GizmosDebug.Selected && Application.isPlaying)
        {
            behaviorMachine.CurrentState.GizmosUpdate();
        }
    }

    private void OnDrawGizmos()
    {
        if (gizmosDebug == GizmosDebug.Always && Application.isPlaying)
        {
             behaviorMachine.CurrentState.GizmosUpdate();
        }
    }
#endif

    #endregion
}