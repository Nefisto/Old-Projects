using System;
using NTools;
using UnityEngine;

[SelectionBase]
public partial class Furniture : LazyBehavior
{
    #region Monobehaviour callbacks

    private void Awake()
    {
        if (light != null)
        {
            SetupLight();
            SetupCollider();
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (!CanDetectEnemies)
            return;

        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.AddDetector(new Detector(gameObject, false));
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (!CanDetectEnemies)
            return;

        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.RemoveDetector(gameObject);
        }
    }

    #endregion  
}