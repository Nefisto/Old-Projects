using System;
using System.Collections;
using System.Numerics;
using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public partial class Player
{
    public event Action OnSuccessfulCastRay;
    public event Action OnFailedToCastRay;

    [FoldoutGroup(FO_Skills)]
    [TabGroup(FO_TAB_Skills, TabName_Ray)]
    [SerializeField]
    private bool debugRay;

    [TabGroup(FO_TAB_Skills, TabName_Ray)]
    [Title("Status")]
    [Tooltip("How much damage this make per trigger?")]
    [SerializeField]
    private float damageRate = 1f;

    [TabGroup(FO_TAB_Skills, TabName_Ray)]
    [Tooltip("How much fuel this consume per second?")]
    [SerializeField]
    private float fuelConsumingRate = 5f;

    [TabGroup(FO_TAB_Skills, TabName_Ray)]
    [Tooltip("How bigger than the original radius the second radius will be")]
    [Range(0f, 10f)]
    [SerializeField]
    private float radiusB = 0f;

    [TabGroup(FO_TAB_Skills, TabName_Ray)]
    [Required]
    [SerializeField]
    private LineRenderer lineRenderer;

    [TabGroup(FO_TAB_Skills, TabName_Ray)]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private Enemy rayTarget;

    [TabGroup(FO_TAB_Skills, TabName_Ray)]
    [ReadOnly]
    [SerializeField]
    private bool isCastingRay;


    #region Properties

    private float RayMaxRadius => visionRadius + radiusB;

    public Enemy RayTarget
    {
        get => rayTarget;
        set => rayTarget = value;
    }

    #endregion

    private IEnumerator rayRoutine;
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

    #region API

    public void StopRay()
    {
        // Enable controllers
        EnableMovement();
        EnableRotation();

        // Allow looking around animation
        animator.SetBool(IsAttacking, false);

        StopCoroutine(rayRoutine);
        RayTarget = null;
        rayRoutine = null;

        lineRenderer.enabled = false;

        isCastingRay = false;

        // Stop any possible "looking at" behavior
        transform.DOKill();
    }

    /// <summary>
    /// This will make the ray follow an enemy and keep looking at it while it move freely
    /// </summary>
    /// <param name="enemy"></param>
    public void CastRayAtTarget()
    {
        // A routine still in execution
        if (rayRoutine != null)
        {
            Debug.LogError("Trying to start a ray without finishing the old one", this);
            return;
        }

        rayRoutine = CastRay();
        StartCoroutine(rayRoutine);
    }

    /// <summary>
    /// Ray will be cast in a direction, damage when touching
    /// </summary>
    /// <param name="updateDirectionRoutine"></param>
    public void CastRayAtDirection (Func<Vector3> updateDirectionRoutine)
    {
        // A routine still in execution
        if (rayRoutine != null)
        {
            Debug.LogError("Trying to start a ray without finishing the old one", this);
            return;
        }

        rayRoutine = CastRay2(updateDirectionRoutine);
        StartCoroutine(rayRoutine);
    }

    #endregion
    
    #region Private Methods

    private void SetupRay()
    {
        lineRenderer.enabled = false;
    }

    private void RegisterRayInput()
    {
        var input = InputController.Input.Gameplay.CastRay;

        input.performed += _ =>
        {
            if (!CanCastRay())
            {
                OnFailedToCastRay?.Invoke();
                return;
            }

            stateMachine.CurrentState.CastRay();
        };

        input.canceled += _ =>
        {
            if (isCastingRay)
                StopRay();
        };
    }

    private void RegisterRayListeners()
    {
        OnSuccessfulCastRay += () =>
        {
            var deltaFuel = fuelConsumingRate * Time.deltaTime;
            UpdateFuel(-deltaFuel);
        };
    }
    
    private IEnumerator CastRay()
    {
        isCastingRay = true;
        EnableLineRenderer();

        DisableMovement();
        DisableRotation();

        animator.SetBool(IsAttacking, true);

        // Look at the enemy
        transform.DOLookAt(RayTarget.transform.position.NTIgnoreY(), .5f);

        while (true)
        {
            if (!CanCastRay())
            {
                StopRay();
                yield break;
            }

            // Calculate values
            var direction = (RayTarget.transform.position - transform.position);
            var distance = direction.magnitude;

            // Wall validation
            if (Physics.Raycast(transform.position, direction.normalized, distance, BlockableLayer))
            {
                StopRay();
                yield break;
            }

            // Validate current distance
            if (distance >= RayMaxRadius)
            {
                if (!TryChangeTarget())
                {
                    StopRay();
                    yield break;
                }

                transform.DOLookAt(RayTarget.transform.position.NTIgnoreY(), .5f);
            }

            // Draw line
            lineRenderer.SetPosition(0, FlashlightPosition.position);
            lineRenderer.SetPosition(1, RayTarget.transform.position);

            // Damage enemy
            var deltaDamage = damageRate * Time.deltaTime;
            RayTarget.TakeDamage(deltaDamage);
            
            OnSuccessfulCastRay?.Invoke();

            yield return null;
        }
    }

    /// <summary>
    /// This will be cast by aim state
    /// </summary>
    /// <param name="updateDirectionRoutine"></param>
    /// <returns></returns>
    private IEnumerator CastRay2 (Func<Vector3> updateDirectionRoutine)
    {
        isCastingRay = true;
        EnableLineRenderer();

        DisableMovement();

        animator.SetBool(IsAttacking, true);

        while (true)
        {
            if (!CanCastRay())
            {
                StopRay();
                yield break;
            }

            var direction = updateDirectionRoutine();

            // Initially at full range
            var rayLenght = RayMaxRadius;
            IDamageable cachedDamageable = null;
            // Has a wall || enemy in ray path?
            if (Physics.Raycast(FlashlightPosition.position, direction, out var hit, rayLenght,
                BlockableLayer | EnemyLayer))
            {
                rayLenght = hit.distance; // RayLenght will decrease if hit a wall in path

                cachedDamageable = hit.collider.GetComponent<IDamageable>();
            }

            // Draw line
            var flashLightPosition = FlashlightPosition.position;
            var endPoint = flashLightPosition + direction * rayLenght;
            lineRenderer.SetPosition(0, flashLightPosition);
            lineRenderer.SetPosition(1, endPoint);

            // Damage enemy
            if (cachedDamageable != null)
            {
                var deltaDamage = damageRate * Time.deltaTime;
                cachedDamageable.TakeDamage(deltaDamage);
            }
            
            OnSuccessfulCastRay?.Invoke();
            
            yield return null;
        }
    }

#if UNITY_EDITOR

    private void DebugRay()
    {
        var originalColor = Handles.color;

        var playerTransform = transform;
        Handles.color = Color.red;
        Handles.DrawWireDisc(playerTransform.position, playerTransform.up, visionRadius + radiusB, 3f);

        Handles.color = originalColor;
    }

#endif

    #endregion

    #region Utilities

    private void EnableLineRenderer()
        => lineRenderer.enabled = true;

    private void DisableLineRenderer()
        => lineRenderer.enabled = false;

    private bool CanCastRay()
    {
        // Can I pay for it
        var deltaFuelSpend = fuelConsumingRate * Time.deltaTime; // Spent fuel on this frame
        if (!HaveEnoughFuel(deltaFuelSpend))
        {
            return false;
        }

        return true;
    }

    private bool TryChangeTarget()
    {
        RayTarget = GetNearestEnemy();

        return RayTarget != null;
    }

    #endregion
    
    #region Raise events

    public virtual void RaiseSuccessfulCastRay()
    {
        OnSuccessfulCastRay?.Invoke();
    }

    public virtual void RaiseFailedToCastRay()
    {
        OnFailedToCastRay?.Invoke();
    }

    #endregion
}