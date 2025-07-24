using Sirenix.OdinInspector;
using UnityEngine;

public abstract partial class BattleActor
{
    [TabGroup("General")]
    [TitleGroup("Settings")]
    [Tooltip("This will be initialized and populated on runtime only.\nDrag inventories here for debug/test reasons only")]
    [SerializeField]
    protected InventoryData inventory;
    
    private void SetupInventory()
    {
#if UNITY_EDITOR
        if (inventory != null)
            return;
#endif
        
        inventory = Data.CreateInventory();
    }
}