using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DragIcon : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Vector3 iconOffset;

    [TitleGroup("References")]
    [SerializeField]
    private Canvas hudCanvas;

    [TitleGroup("References")]
    [SerializeField]
    private Image icon;

    private NTask followingRoutine;

    private void Start()
    {
        icon.gameObject.SetActive(false);
        followingRoutine = new NTask(FollowingMouseRoutine(), new NTask.Settings { autoStart = false });
    }

    private IEnumerator FollowingMouseRoutine()
    {
        while (true)
        {
            var screenPoint = Input.mousePosition + iconOffset;
            var worldPoint = hudCanvas.worldCamera.ScreenToWorldPoint(screenPoint);
            worldPoint.z = hudCanvas.worldCamera.nearClipPlane;

            icon.transform.position = worldPoint;

            yield return null;
        }
    }

    public void EnableFollowingRoutine()
    {
        icon.sprite = ServiceLocator.GameContext.TurnContext.SelectedCard.Icon;
        icon.gameObject.SetActive(true);
        followingRoutine.Resume();
    }

    public void DisableFollowingRoutine()
    {
        icon.gameObject.SetActive(false);
        followingRoutine.Pause();
    }
}