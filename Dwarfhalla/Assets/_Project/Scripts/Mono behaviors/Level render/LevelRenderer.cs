using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable RedundantAssignment

public class LevelRenderer : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Transform levelFolder;

    [TitleGroup("References")]
    [SerializeField]
    private RuntimeBlock blockPrefab;

    [TitleGroup("References")]
    [SerializeField]
    private Material blackMaterial;

    private void Awake() => GameEntryPoints.OnRenderingLevel += RenderLevel;

    private void Start() => levelFolder.DestroyChildren();

    [Button]
    private void Create()
    {
        StartCoroutine(RenderLevel(null));
    }

    [Button]
    private void Try()
    {
        var roomFolder = new GameObject($"Room N")
        {
            transform =
            {
                parent = levelFolder,
                position = Vector3.zero,
                localPosition = Vector3.zero
            }
        };
    }

    private IEnumerator RenderLevel (object _)
    {
        levelFolder.DestroyChildren();

        var roomData = ServiceLocator.GameContext.LevelData.CurrentRoom;
        var roomFolder = new GameObject($"Room {ServiceLocator.GameContext.LevelData.CurrentLevelIndex + 1}")
        {
            transform =
            {
                parent = levelFolder,
                localPosition = Vector3.zero
            }
        };

        var allPositions = GetAllPosition(roomData.RoomSize.x, roomData.RoomSize.y);
        var positionToFinishAnimation = allPositions
            .ToDictionary(p => p, _ => false);
        foreach (var (x, y) in allPositions.Shuffle())
        {
            var isWhiteBlock = (x + y) % 2 == 0;
            var blockData = roomData.GetBlock(x, y);

            var endPosition = new Vector3(x, 0f, y);
            var blockInstance = Instantiate(blockPrefab, endPosition + new Vector3(0f, 10f, 0f),
                Quaternion.identity, roomFolder.transform);

            blockInstance.Setup(new RuntimeBlock.SetupSettings
            {
                blockData = blockData,
                isWhiteBlock = isWhiteBlock
            });

            blockInstance
                .transform
                .DOLocalMoveY(isWhiteBlock ? 0 : 0.05f, 1f)
                .SetEase(Ease.OutElastic)
                .OnComplete(() => positionToFinishAnimation.Remove((x, y)));

            blockData.RuntimeBlock = blockInstance;
            yield return null;
        }

        yield return new WaitUntil(() => positionToFinishAnimation.Count == 0);

        yield return GameEntryPoints.OnRenderedLevel?.YieldableInvoke();
    }

    private List<(int, int)> GetAllPosition (int lines, int columns)
    {
        var mixedPositions = new List<(int, int)>();

        for (var i = 0; i < lines; i++)
        for (var j = 0; j < columns; j++)
            mixedPositions.Add((i, j));

        return mixedPositions;
    }
}