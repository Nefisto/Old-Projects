using System;
using System.Collections;
using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class Enemy
{
    public Action OnEnemyDie;

    [TabGroup("Life")]
    [SerializeField]
    private float maxLife = 100f;

    [TabGroup("Life")]
    [Title("Debug")]
    [SerializeField]
    private GameObject lifeMeter;

    [TabGroup("Life")]
    [ReadOnly]
    [SerializeField]
    private float currentLife;

    // Debug reasons only, pivot value to change the life meter in a percentage way
    private float lifeMeterInitialSize;

    // Used to lock meter gained every frame
    private bool tookDamageThisTurn = false;
    private static readonly int Die1 = Animator.StringToHash("Die");

    // TODO: Urgent -> Transfer detection behavior to base STATE, this way I can just make the DIE state override it and not call the base behavior
    private bool IsDied = false;
    
    private void SetupLife()
    {
        lifeMeterInitialSize = lifeMeter.transform.localScale.x;

        currentLife = maxLife;

        OnEnemyDie += () => IsDied = true;
    }

    // From interface
    public void TakeDamage (float damage)
    {
        if (!isVisible)
        {
            UpdateDetect(damage);

            tookDamageThisTurn = true;
        }
        else
        {
            UpdateLife(damage);
        }
    }

    private void UpdateLife(float amount)
    {
        currentLife = Mathf.Clamp(currentLife - amount, 0f, maxLife);

        var percentage = currentLife / maxLife;
        
        ResizeLifeMeter(percentage);

        if (currentLife.IsNearlyEnoughTo(0f))
        {
            Die();
        }
    }
    
    private void ResizeLifeMeter (float percentage)
    {
        var newSize = lifeMeterInitialSize * percentage;

        var scale = lifeMeter.transform.localScale;
        lifeMeter.transform.localScale = new Vector3(newSize, scale.y, scale.z);
    }

    [Button]
    public void Die()
    {
        ChangeState(EnemyStates.Die);
    }

    public IEnumerator FadeOut ()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}