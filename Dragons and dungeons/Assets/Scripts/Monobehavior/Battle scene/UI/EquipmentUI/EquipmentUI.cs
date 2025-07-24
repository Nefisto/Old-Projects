using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EquipmentUI : MonoBehaviour, IEnumerable<Slot>, IDroppableArea
{
    [Title("Control")]
    [SerializeField]
    private WeaponSlot weaponSlot;

    [SerializeField]
    private HeadSlot headSlot;

    [SerializeField]
    private ChestSlot chestSlot;

    [Title("Debug")]
    [SerializeField]
    private ActorData currentActor;

    private void Start()
    {
        ConfigureSlots();

        if (currentActor != null)
            ChangeEquipment(currentActor);

        BattleManager.Instance.OnSpawnActors += RegisterOnAlliesTurnStartFinished;
        BattleManager.Instance.OnSpawnActors += RegisterToDisableEquipment;
    }

    private void RegisterToDisableEquipment()
    {
        var allies = BattleManager.Instance.GetPlayerCharacters();

        foreach (var ally in allies)
        {
            ally.OnEndTurn += _ =>
            {
                MouseController.Instance.CancelDrag();
                DisableAllEquipments();
            };

            ally.OnRunSkill += _ =>
            {
                MouseController.Instance.CancelDrag();
                DisableAllEquipments();
            };
        }
    }

    private void DisableAllEquipments()
    {
        foreach (var slot in this)
            slot.DisableSlot();
    }

    private void EnableAllEquipments()
    {
        foreach (var slot in this)
            slot.EnableSlot();
    }

    private void RegisterOnAlliesTurnStartFinished()
    {
        var allies = BattleManager.Instance.GetPlayerCharacters();

        foreach (var ally in allies)
        {
            ally.OnFinishStartTurn += _ => EnableAllEquipments(); 
            ally.OnFinishStartTurn += ctx => ChangeEquipment(ctx.Actor.Data);
        }
    }

    public void ChangeEquipment (ActorData actor)
    {
        if (currentActor != null)
            currentActor.OnChangeEquipment -= OnChangeEquipListener;

        currentActor = actor;
        currentActor.OnChangeEquipment += OnChangeEquipListener;

        DrawEquipment();
    }

    public void DrawEquipment()
    {
        Clear();

        if (currentActor == null)
            return;

        weaponSlot.UpdateSlot(new InventoryItem(currentActor.Equipment.weapon));
        headSlot.UpdateSlot(new InventoryItem(currentActor.Equipment.headArmor));
        chestSlot.UpdateSlot(new InventoryItem(currentActor.Equipment.chestArmor));
    }

    public void Clear()
    {
        weaponSlot.ClearSlot();
        headSlot.ClearSlot();
        chestSlot.ClearSlot();
    }

    public ChangeResult Change (ChangeItemContext ctx)
    {
        var itemData = ctx.Item.data;
        switch (ctx.Slot)
        {
            case WeaponSlot _:
                currentActor.EquipWeapon(itemData as Weapon);
                break;

            case HeadSlot _:
                currentActor.EquipHead(itemData as HeadArmor);
                break;

            case ChestSlot _:
                currentActor.EquipChestArmor(itemData as ChestArmor);
                break;

            default:
                throw new Exception($"Trying to equip an undefined type of item. {itemData}");
        }

        return new ChangeResult();
    }

    public bool IsValid (ChangeItemContext ctx)
        => true;

    private void OnChangeEquipListener() => DrawEquipment();

    private void ConfigureSlots()
    {
        weaponSlot.droppableArea = this;
        headSlot.droppableArea = this;
        chestSlot.droppableArea = this;
    }

    public IEnumerator<Slot> GetEnumerator()
    {
        yield return weaponSlot;
        yield return headSlot;
        yield return chestSlot;
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}