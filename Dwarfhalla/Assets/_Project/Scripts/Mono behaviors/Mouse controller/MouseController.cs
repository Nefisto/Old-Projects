using System;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Action mouseButtonUpOperation;

    private Action onHoveringOnNoBlocks;

    private void Awake() => ServiceLocator.MouseController = this;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            mouseButtonUpOperation?.Invoke();

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 50, LayerMask.GetMask("Block")))
            onHoveringOnNoBlocks?.Invoke();
    }

    public void SetMouseButtonUpOperation (Action operation) => mouseButtonUpOperation = operation;
    public void RemoveMouseButtonUpOperation() => mouseButtonUpOperation = null;

    public void SetupOnHoveringOnTopOfNothing (Action operation) => onHoveringOnNoBlocks = operation;
    public void RemoveOnHoveringOnNoBlocksOperation() => onHoveringOnNoBlocks = null;
}