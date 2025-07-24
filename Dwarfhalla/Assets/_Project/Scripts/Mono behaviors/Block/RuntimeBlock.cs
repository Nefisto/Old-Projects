using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public partial class RuntimeBlock : MonoBehaviour
{
    public enum NotificationType
    {
        Damage,
        Healer,
        Neutral,
        ImpossibleBlock,
        PossibleBlock,
        HoveringBlock
    }

    [TitleGroup("Settings")]
    [SerializeField]
    private NDictionary<NotificationType, NotificationSettings> notifyTypeToColorGradient = new();

    [TitleGroup("References")]
    [SerializeField]
    private MeshRenderer notifierRenderer;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    [TitleGroup("References")]
    [SerializeField]
    private GameObject whiteBlock;

    [TitleGroup("References")]
    [SerializeField]
    private GameObject blackBlock;

    [field: TitleGroup("References")]
    [field: Tooltip("Position where piece should stick")]
    [field: SerializeField]
    public Transform PiecePosition { get; private set; }

    [TitleGroup("References")]
    [Tooltip("Just to show position on scene")]
    [SerializeField]
    private GameObject debugPosition;

    private Action clickOperation;
    private Action hoverOperation;
    private NTask notifyRoutine;

    [field: TitleGroup("Debug")]
    [field: ReadOnly]
    [field: ShowInInspector]
    public BlockData BlockData { get; private set; }

    private NTask hoveringDetailRoutine;
    
    public void Setup (SetupSettings settings)
    {
        BlockData = settings.blockData;
        SetupBlockColor(settings.isWhiteBlock);

        PiecePosition.DestroyChildren();
        if (settings.blockData.InitialUnitOnThisBlock != null)
            SetupModel(settings.blockData.InitialUnitOnThisBlock);

        HideNotifier();
        SetupMouseOperations();
        
        debugPosition.name = $"{BlockData.Position.x}, {BlockData.Position.y}";
    }

    private void SetupBlockColor (bool isWhiteBlock)
    {
        whiteBlock.SetActive(false);
        blackBlock.SetActive(false);

        (isWhiteBlock ? whiteBlock : blackBlock).SetActive(true);
    }

    private void SetupModel (UnitData piece)
    {
        BlockData.UnitData = piece;

        piece.RuntimeUnit = Instantiate(piece.ModelPrefab, PiecePosition.position, piece.ModelPrefab.transform.rotation,
            PiecePosition);
        piece.Setup(BlockData);
    }

    private void SetupMouseOperations()
    {
        var mouseEnterEntry = new EventTrigger.Entry();
        mouseEnterEntry.eventID = EventTriggerType.PointerEnter;
        mouseEnterEntry.callback.AddListener(_ =>
        {
            hoverOperation?.Invoke();

            if (!BlockData.HasUnitOnIt)
                return;

            hoveringDetailRoutine = new NTask(HoveringDetailRoutine());
        });

        var hoverExitEntry = new EventTrigger.Entry();
        hoverExitEntry.eventID = EventTriggerType.PointerExit;
        hoverExitEntry.callback.AddListener(_ =>
        {
            hoveringDetailRoutine?.Stop();
            ServiceLocator.Tooltip.HideTooltip();
        });
        
        var clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener(data =>
        {
            if (((PointerEventData)data).button != PointerEventData.InputButton.Left)
                return;

            clickOperation?.Invoke();
        });

        eventTrigger.triggers.Add(hoverExitEntry);
        eventTrigger.triggers.Add(mouseEnterEntry);
        eventTrigger.triggers.Add(clickEntry);
    }

    private IEnumerator HoveringDetailRoutine()
    {
        yield return new WaitForSeconds(0.75f);

        if (!BlockData.HasUnitOnIt)
            yield break;
        
        ServiceLocator.Tooltip.Setup(ServiceLocator.Database.GetCardFromUnit(BlockData.UnitData));
        ServiceLocator.Tooltip.ShowTooltip();
    }
    
    public void RemoveClickOperation() => clickOperation = null;

    public void RemoveHoverOperation() => hoverOperation = null;

    public void SetHoverOperation (Action hoverOperation) => this.hoverOperation = hoverOperation;
    public void SetClickOperation (Action clickOperation) => this.clickOperation = clickOperation;

    [TitleGroup("Operations")]
    [Button]
    public void CancelNotification()
    {
        notifyRoutine?.Stop();
        HideNotifier();
    }

    public void CacheNotification() => cachedNotification = currentNotification;

    public void LoadNotification() => Notify(cachedNotification);

    public void Notify (NotificationType notificationType)
    {
        notifyRoutine?.Stop();

        currentNotification = notificationType;
        notifyRoutine = new NTask(InternalNotify(notificationType));
    }

    public class SetupSettings
    {
        public BlockData blockData;
        public bool isWhiteBlock;
    }

    [Serializable]
    public class NotificationSettings
    {
        public Texture texture;
        public Gradient gradient;

        public float blinkingSpeed = 1.5f;
    }
}