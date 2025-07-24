using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ManuallyTargetSelector : MonoBehaviour, ITargetSelector
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Vector3 offset;

    [TitleGroup("References")]
    [SerializeField]
    private Image targetIcon;

    private void Awake() => ServiceLocator.TargetSelector = this;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public EnemyBattleActor CurrentTarget { get; private set; }

    public void SetTarget (EnemyBattleActor target)
    {
        CurrentTarget = target == CurrentTarget
            ? null
            : target;
        targetIcon.enabled = CurrentTarget != null;

        if (CurrentTarget == null)
            return;

        var targetTransform = CurrentTarget.transform;
        var iconTransform = targetIcon.transform;

        var multiplier = targetTransform.localScale.x;
        iconTransform.localScale = targetTransform.localScale;
        iconTransform.position = targetTransform.position + (offset * multiplier);
    }

    public void Clear() => SetTarget(null);
}