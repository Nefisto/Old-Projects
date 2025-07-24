using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Awake()
    {
        GameEntryPoints.OnSetupScene += AdjustCameraPosition;
    }

    private IEnumerator AdjustCameraPosition (object _)
    {
        var context = ServiceLocator.GameContext;
        var roomSize = context
            .LevelData
            .CurrentRoom
            .RoomSize;
        transform.position = new Vector3(roomSize.x * .5f - .5f, 0f, roomSize.y * .5f - .5f);
        yield break;
    }
}