using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.InventoryName, menuName = Nomenclature.InventoryMenu, order = 0)]
public class InventoryData : ScriptableObject, IEnumerable<InventoryItem>, ICloneable
{
    public event Action<AddItemContext> OnAddItem;
    public event Action<RemoveItemContext> OnRemoveItem;

    /// <summary>
    /// This could be done with an fixed array size (which would simplify things) but project owner want to (probably)
    ///     implement an bag feature (as we have in WoW), an with the InventoryItem abstraction its easy to change to
    ///     something else.
    /// </summary>
    [SerializeField]
    private List<InventoryItem> inventory = new List<InventoryItem>(new InventoryItem[GameConstants.General.MaxItemsOnInventory]);

    public int Count => inventory.Count;

    public bool CanStack (InventoryItem item, int index)
        => item.CanStack && inventory[index]?.data == item.data;

    public void Stack (InventoryItem item, int index)
    {
        inventory[index].amount += item.amount;

        OnAddItem?.Invoke(new AddItemContext(item));
    }

    public void Add (InventoryItem item, int index)
    {
        inventory[index].data = item.data;
        inventory[index].amount = item.amount;
    }

    public void ClearSlot (int index)
    {
        inventory[index].data = null;
        inventory[index].amount = 0;
    }

    public bool Remove (InventoryItem item)
    {
        var hasRemoved = inventory.Remove(item);

        if (hasRemoved)
            OnRemoveItem?.Invoke(new RemoveItemContext() { Item = item });

        return hasRemoved;
    }

    public void Insert (int index, InventoryItem item)
        => inventory.Insert(index, item);

    public IEnumerator<InventoryItem> GetEnumerator()
        => inventory.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public void Clear()
    {
        inventory = new List<InventoryItem>(new InventoryItem[GameConstants.General.MaxItemsOnInventory]);

        if (Application.isPlaying)
            OnRemoveItem?.Invoke(new RemoveItemContext());
    }

    public bool Contains (InventoryItem item)
        => inventory.Contains(item);

    public void CopyTo (InventoryItem[] array, int arrayIndex)
    {
        for (var i = 0; i < Count; i++)
            array[arrayIndex++] = inventory[i];
    }

    public int IndexOf (InventoryItem item)
        => inventory.IndexOf(item);

    // DRY on Remove
    public void RemoveAt (int index)
    {
        if (index < 0 || index >= inventory.Count)
            throw new ArgumentOutOfRangeException();

        var item = inventory[index];
        Remove(item);
    }

    public InventoryItem this [int index]
    {
        get => inventory[index];
        set => inventory[index] = value;
    }

    public object Clone()
    {
        var instance = CreateInstance<InventoryData>();

        for (var i = 0; i < inventory.Count; i++)
            instance.inventory[i] = (InventoryItem)inventory[i].Clone();

        return instance;
    }

    public static InventoryData GetEmptyInventory()
        => CreateInstance<InventoryData>();

#if UNITY_EDITOR

    [PropertyOrder(-10)]
    [Button]
    private void ClearInventory()
        => Clear();

    [PropertyOrder(-10)]
    [Button]
    private void AddItem (ItemData item, int amount = 1)
    {
        var firstEmptySlot = inventory.FirstOrDefault(i => i.data == null);

        if (firstEmptySlot != null)
        {
            firstEmptySlot.data = item;
            firstEmptySlot.amount = amount;

            if (Application.isPlaying)
                OnAddItem?.Invoke(new AddItemContext(new InventoryItem(item, amount)));
        }
        
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}