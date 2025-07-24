using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectsToHide : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private List<GameObject> objectsToHide;

    private void Awake()
    {
        GameEntryPoints.OnSetupScene += _ => DisableObjects();
        GameEntryPoints.OnFinishedSetup += _ => EnableObjects();
    }

    private void DisableObjects() => objectsToHide.ForEach(obj => obj.SetActive(false));
    private void EnableObjects() => objectsToHide.ForEach(obj => obj.SetActive(true));
}