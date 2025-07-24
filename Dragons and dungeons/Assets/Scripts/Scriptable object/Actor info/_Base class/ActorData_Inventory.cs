using Sirenix.OdinInspector;
using UnityEngine;

public partial class ActorData
{
    [PropertyOrder(10)]
    [TabGroup("Data tab", "Equipment")]
    [Title("Initial inventory")]
    [SerializeField]
    private InventoryData inventory;

    public InventoryData CreateInventory()
    {
        if (inventory == null)
            return InventoryData.GetEmptyInventory();

        return (InventoryData)inventory.Clone();
    }
}