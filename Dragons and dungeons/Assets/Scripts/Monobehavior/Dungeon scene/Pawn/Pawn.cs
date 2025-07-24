using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [Title("Settings")]
    [MinValue(.5f)]
    [SerializeField]
    private float pawnSpeed = 5f;
    
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private DungeonNode currentNode;

    public void SetPosition (Vector2 targetPosition)
        => transform.position = targetPosition;

    public IEnumerator GoToNextNode()
    {
        if (currentNode.IsEndNode())
        {
            yield return new WaitForSeconds(1.5f);
            GameEvents.DungeonMap.RaiseFinishDungeon();
            
            yield break;
        }
        
        yield return GoToNode(currentNode.next);
    }
    
    public IEnumerator GoToNode (DungeonNode dungeonNode)
    {
        currentNode = dungeonNode;
        yield return WalkTo(dungeonNode.transform.position);

        yield return new WaitForSeconds(1f);

        currentNode.Reached();
    }
    
    private IEnumerator WalkTo(Vector2 targetPosition)
    {
        var duration = Vector2.Distance(transform.position, targetPosition) / pawnSpeed;
        var moveTween = transform
            .DOMove(targetPosition, duration)
            .SetEase(Ease.Linear);

        yield return moveTween.WaitForCompletion();
    }
    
    
#if UNITY_EDITOR
    [DisableInEditorMode]
    [PropertyTooltip("Will try to found a node in a radius")]
    [Button]
    private void ScanNode()
    {
        var hit = Physics2D.CircleCast(transform.position, 1f, Vector2.zero);
        if (hit.collider.TryGetComponent<DungeonNode>(out var node))
            node.Reached();
    }
    
    [Button]
    public void StartWalkTo(Transform target)
    {
        StartCoroutine(WalkTo(target.position));
    }
#endif
}