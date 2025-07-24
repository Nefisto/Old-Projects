using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Fireball : LazyBehavior
{
    private const string TabBasic = "Basic";

    [TabGroup(TabBasic)]
    [SerializeField]
    private bool isDebugging;
    
    [TabGroup(TabBasic)]
    [Title("Status")]
    [SerializeField]
    private int damage = 5;
    
    [TabGroup(TabBasic)]
    [SerializeField]
    private float movementSpeed = 5f;
    
    [TabGroup(TabBasic)]
    [Tooltip("The function that will define how the fireball will get smaller")]
    [SerializeField]
    private AnimationCurve reductionFunction;

    [TabGroup(TabBasic)]
    [Tooltip("Total amount of time that fireball should live")]
    [SerializeField]
    private float lifeSpan = 3f;

    [TabGroup(TabBasic)]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private Enemy target;

    [TabGroup(TabBasic)]
    [ReadOnly]
    [SerializeField]
    private float initialRadius;

    [TabGroup(TabBasic)]
    [ReadOnly]
    [SerializeField]
    private Vector3 initialScale;

    [TabGroup(TabBasic)]
    [ReadOnly]
    [SerializeField]
    private float lifeTime;

    [TabGroup(TabBasic)]
    [ReadOnly]
    [SerializeField]
    private float currentRadius;

    [TabGroup(TabBasic)]
    [ReadOnly]
    [SerializeField]
    private Vector3 direction;
    
    // Will follow some target
    private bool hasTarget = false;
    
    private Task followTask;
    
    #region Properties

    private float CurrentRadiusPercentage => currentRadius / initialRadius;
    private float CurrentLifeTimePercentage => lifeTime / lifeSpan;

    #endregion

    #region Monobehaviour callbacks

    private void Awake()
    {
        initialRadius = sphereCollider.radius;
        initialScale = transform.localScale;
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
            
            Explode();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!target)
            return;
        
        Handles.color = Color.red;
        Handles.DrawAAPolyLine(Texture2D.whiteTexture, transform.position, target.transform.position);
    }
#endif
    
    #endregion

    #region API

    [Button("With direction")]
    [DisableInEditorMode]
    public void Setup (Vector3 dir)
    {
        // Init status
        currentRadius = initialRadius;
        target = null;
        direction = dir;
        hasTarget = false;
        
        // Register on enemy die event
        
        // Init task
        followTask = new Task(FollowingTask());

    }
    
    [Button("With target")]
    [DisableInEditorMode]
    public void Setup(Enemy target)
    {
        // Init status
        currentRadius = initialRadius; 
        this.target = target;
        direction = transform.forward;
        hasTarget = true;
        
        // Register on enemy die event
        
        // Init task
        followTask = new Task(FollowingTask());
    }

    #endregion

    #region Private

    private IEnumerator FollowingTask()
    {
#if UNITY_EDITOR // Debugging purposes
        var initialPos = transform.position;
#endif
        
        while (lifeTime <= lifeSpan)
        {
            lifeTime += Time.deltaTime;
            
            // Calculate the delta radius decreasing
            var percentage = reductionFunction.Evaluate(CurrentLifeTimePercentage);
            Resize(percentage);
            
            // Calculate the direction to move
            var position = transform;
            var dir = hasTarget
                ? (target.transform.position - position.position).normalized
                : direction;
            
            transform.Translate(dir * movementSpeed * Time.deltaTime, Space.World);
            
            yield return null;
        }

#if UNITY_EDITOR // Debugging pursposes
        if (isDebugging)
        {
            transform.position = initialPos;
            Resize(1f);
            followTask.Stop();
            lifeTime = 0f;
            yield break;
        }
#endif
        Explode();
    }

    private void Resize (float percentage)
    {
        // Adjust the scale
        transform.localScale = initialScale * percentage;
        
        // Adjust the radius
        sphereCollider.radius = initialRadius * percentage;
    }

    private void Explode()
    {
        followTask.Stop();
        
        Destroy(gameObject);
    }

    #endregion
}