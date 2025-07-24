using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.HealthPotionName, menuName = Nomenclature.HealthPotionMenu)]
public class HealthPotion : ItemData
{
    [field: SerializeField]
    public override bool CanStack { get; protected set; } = true;

    protected override ItemData CreateNewInstance => CreateInstance<HealthPotion>();
}