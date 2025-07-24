using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetClickDetector : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    [TitleGroup("References")]
    [SerializeField]
    private Camera battleCamera;

    private void Start()
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(a =>
        {
            var results2D = new RaycastHit2D[50];
            var clickPosition = battleCamera.ScreenToWorldPoint(((PointerEventData)a).position);
            var size = Physics2D.RaycastNonAlloc(clickPosition, Vector2.zero, results2D);

            if (size <= 1)
                return;

            var nearestToClickEnemy = results2D
                .Take(size)
                .Select(r =>
                {
                    r.collider.TryGetComponent<EnemyBattleActor>(out var battleActor);

                    return battleActor;
                })
                .Where(ba => ba is not null)
                .OrderBy(ba => Vector2.SqrMagnitude(clickPosition - ba.transform.position))
                .First();

            ServiceLocator.TargetSelector.SetTarget(nearestToClickEnemy);
        });

        eventTrigger.triggers.Clear();
        eventTrigger.triggers.Add(entry);
    }
}