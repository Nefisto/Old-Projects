using System;
using System.Collections;
using DG.Tweening;
using QFSW.QC;
using Sirenix.OdinInspector;
using UnityEngine;

[CommandPrefix("Camera.")]
public class SmoothCameraZoom : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private float zoomSpeed = 10f;

    [TitleGroup("Settings")]
    [Command("min-zoom")]
    [Range(1f, 5f)]
    [SerializeField]
    private float minZoom = 2f;

    [TitleGroup("Settings")]
    [Command("max-zoom")]
    [Range(3f, 10f)]
    [SerializeField]
    private float maxZoom = 7f;

    [TitleGroup("Settings")]
    [SerializeField]
    private float zoomLerpDuration = 0.2f;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private bool canZoom = true;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float percentage;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float lastScrollInput;
    
    private Camera mainCamera;
    private Transform cameraParentFolder;
    
    private Coroutine zoomCoroutine;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraParentFolder = mainCamera.transform.parent;
        
        percentage = (mainCamera.orthographicSize - minZoom) / (maxZoom - minZoom); 
    }

    private void Update()
    {
        if (!canZoom)
            return;
        
        lastScrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetMouseButtonDown(2))
            StartCoroutine(Zoom(Vector3.zero, maxZoom));

        if (lastScrollInput == 0)
            return;
        
        if (lastScrollInput > 0 && !CanZoomIn())
            return;
        
        if (lastScrollInput < 0 && !CanZoomOut())
            return;
        
        if (Mathf.Abs(lastScrollInput) > 0.01f)
        {
            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            zoomCoroutine = StartCoroutine(SmoothZoom(lastScrollInput));
        }
    }

    private bool CanZoomOut() => !Mathf.Approximately(mainCamera.orthographicSize, maxZoom);

    private bool CanZoomIn() => !Mathf.Approximately(mainCamera.orthographicSize, minZoom);

    private IEnumerator SmoothZoom (float scrollInput)
    {
        percentage = Mathf.Clamp(percentage + scrollInput, 0f, 1f);
        
        var initialZoom = mainCamera.orthographicSize;
        var targetZoom = Mathf.Clamp(initialZoom - scrollInput * zoomSpeed, minZoom, maxZoom);

        var targetPosition = scrollInput > 0 ? CalculateTargetPosition() : Vector3.zero;

        yield return Zoom(targetPosition, targetZoom);
    }

    private IEnumerator Zoom (Vector3 targetPosition, float targetZoom)
    {
        canZoom = false;

        yield return DOTween
            .Sequence()
            .Append(cameraParentFolder.DOMove(targetPosition, zoomLerpDuration))
            .Join(DOTween.To(
                () => mainCamera.orthographicSize, 
                x => mainCamera.orthographicSize = x, 
                targetZoom, 
                zoomLerpDuration))
            .WaitForCompletion();
        
        canZoom = true;
    }

    private Vector3 CalculateTargetPosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var targetPosition = Vector3.zero;

        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (!groundPlane.Raycast(ray, out var enter))
            return targetPosition;
        
        var hitPoint = ray.GetPoint(enter);
        hitPoint = new Vector3(Mathf.Clamp(hitPoint.x, 0f, 4f), 0f, Mathf.Clamp(hitPoint.z, 0f, 4f));
            
        targetPosition = hitPoint;

        return targetPosition;
    }
}