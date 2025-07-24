using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// We can drop items in different places like: Inventory, Equipment, Battle results HUD
///     and each place should act in a different way.
/// </summary>
public interface IDroppableArea
{
    ChangeResult Change (ChangeItemContext ctx);
    bool IsValid (ChangeItemContext ctx);

}
public class ChangeResult
{
    public bool successfullyEquiped = false;
    public bool HasStacked;
}

public class InventoryUI : MonoBehaviour, IDroppableArea
{
    [Title("Debug")]
    [SerializeField]
    private InventoryData inventoryData;
    
    [ReadOnly]
    [SerializeField]
    private List<Slot> slots;
    
    private void Start()
    {
        CacheSlots();
        ConfigureSlots();

        if (inventoryData != null)
            ChangeInventory((InventoryData)inventoryData.Clone());

        BattleManager.Instance.OnSpawnActors += RegisterOnAlliesTurnStart;
        BattleManager.Instance.OnSpawnActors += RegisterToDisableInventory;
    }

    private void RegisterToDisableInventory()
    {
        var allies = BattleManager.Instance.GetPlayerCharacters();

        foreach (var ally in allies)
        {
            ally.OnEndTurn += _ => slots.ForEach(s => s.DisableSlot());
            ally.OnRunSkill += _ => slots.ForEach(s => s.DisableSlot());
        }
    }

    public ChangeResult Change (ChangeItemContext ctx)
    {
        var result = new ChangeResult();

        var item = ctx.Item;
        var index = ctx.Slot.Index;
        if (ctx.HasEmptyItem)
        {
            inventoryData.ClearSlot(index);
            DrawInventory();
            return result;
        }

        if (inventoryData.CanStack(item, index))
        {
            inventoryData.Stack(item, index);
            result.HasStacked = true;

            DrawInventory();
            return result;
        }
        
        inventoryData.Add(ctx.Item, index);

        DrawInventory();
        return result;
    }

    public bool IsValid (ChangeItemContext _)
        => true;

    [DisableInEditorMode]
    [Button]
    public void ChangeInventory (InventoryData inventory)
    {
        if (inventoryData != null)
        {
            inventoryData.OnAddItem -= OnAddItemListener;
            inventoryData.OnRemoveItem -= OnRemoveItemListener;
        }

        inventoryData = inventory;
        inventoryData.OnAddItem += OnAddItemListener;
        inventoryData.OnRemoveItem += OnRemoveItemListener;

        DrawInventory();
    }

    private void OnAddItemListener (AddItemContext _)
        => DrawInventory();

    private void OnRemoveItemListener (RemoveItemContext _)
        => DrawInventory();

    private void RegisterOnAlliesTurnStart()
    {
        var allies = BattleManager.Instance.GetPlayerCharacters();

        foreach (var ally in allies)
        {
            ally.OnFinishStartTurn += ctx =>
            {
                ChangeInventory(ctx.InventoryData);
                slots.ForEach(s => s.EnableSlot());;
            };
        }
    }
    
    [DisableInEditorMode]
    [Button]
    private void DrawInventory()
    {
        CleanInventory();

        for (var i = 0; i < inventoryData.Count; i++)
            slots[i].UpdateSlot(inventoryData[i]);
    }

    [DisableInEditorMode]
    [Button]
    private void CleanInventory()
        => slots.ForEach(s => s.ClearSlot());

    private void CacheSlots()
        => slots = GetComponentsInChildren<Slot>().ToList();

    private void ConfigureSlots()
    {
        for (var i = 0; i < slots.Count; i++)
        {
            slots[i].droppableArea = this;
            slots[i].SetPosition(i);
        }
    }
}