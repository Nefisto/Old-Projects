using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private float moveSpeed;

    /// <summary>
    ///  We raise TickEncounter when moving for some time
    /// </summary>
    [TitleGroup("Settings")]
    [SerializeField]
    private float timeMovingToTickEncounter = .5f;

    [TitleGroup("References")]
    [SerializeField]
    private Animator animator;

    [TitleGroup("References")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private bool canMove = true;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private float counterToTickEncounter;

    private void Awake()
    {
        LockMovement();

        GameEvents.OnBegunAdventureEntryPoint += UnlockMovement;
        GameEvents.OnGameOver += LockMovement;
        GameEvents.onBattleTriggered += _ => LockMovement();
        GameEvents.OnBattleFinishedEntryPoint += _ =>
        {
            if (Blackboard.GameInfo.RemainingTries <= 0)
                return;

            UnlockMovement();
        };

        GameEvents.onGameStart += () =>
        {
            ServiceLocator.MenuStack.OnOpenFirstMenu += LockMovement;
            ServiceLocator.MenuStack.OnCloseAllMenus += UnlockMovement;
        };
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        var horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        var vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(x: horizontal, y: vertical, z: 0f));

        if (horizontal == 0 && vertical == 0)
            animator.speed = 0;

        if (horizontal != 0 || vertical != 0)
        {
            counterToTickEncounter += Time.deltaTime;

            spriteRenderer.flipX = horizontal < 0;
            
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
            animator.speed = 1;
        }

        if (!(counterToTickEncounter >= timeMovingToTickEncounter))
            return;

        GameEvents.OnTickEncounter?.Invoke();
        counterToTickEncounter = 0f;
    }

    private void LockMovement() => canMove = false;
    private void UnlockMovement() => canMove = true;
}