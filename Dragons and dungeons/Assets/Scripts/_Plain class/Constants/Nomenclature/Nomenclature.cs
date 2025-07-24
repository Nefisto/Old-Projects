/// <summary>
/// This will control how names will appear in right click and menu contexts on editor
/// </summary>
public static class Nomenclature
{
    // BaseMenu + "/" + SkillMenu + "/" + BasicAttackName;

    private const string BaseMenu = "DnD";

    #region Player

    public const string PlayerStatusName = "Player";
    public const string PlayerStatusMenu = BaseMenu + "/" + PlayerStatusName;

    #endregion

    #region Data

    public const string PlayableCharacterName = "Playable character data";
    public const string PlayableCharacterMenu = BaseMenu + "/Data/" + PlayableCharacterName;
    public const string EnemyStatusName = "Enemy data";
    public const string EnemyStatusMenu = BaseMenu + "/Data/" + EnemyStatusName;

    #endregion

    #region Groups

    public const string EnemyGroupName = "Enemy group";
    public const string EnemyGroupMenu = BaseMenu + "/" + EnemyGroupName;

    #endregion

    #region Skills

    private const string SkillMenu = "Skills";

    public const string BasicAttackName = "Basic Attack";
    public const string BasicAttackMenu = BaseMenu + "/" + SkillMenu + "/" + BasicAttackName;

    public const string HealName = "Heal";
    public const string HealMenu = BaseMenu + "/" + SkillMenu + "/" + HealName;

    public const string ModifierApplierName = "Modifier Applier";
    public const string ModifierApplierMenu = BaseMenu + "/" + SkillMenu + "/" + ModifierApplierName;

    #endregion

    #region Modifiers

    private const string ModifierMenu = "Modifier";

    public const string StatusModifierName = "Data modifier";
    public const string StatusModifierMenu = BaseMenu + "/" + ModifierMenu + "/" + StatusModifierName;

    #endregion

    #region Status change

    public const string StatusChangeName = "Passive status change";
    public const string StatusChangeMenu = BaseMenu + "/" + SkillMenu + "/" + StatusChangeName;

    #endregion
    
    #region Equipment

    private const string EquipmentMenu = BaseMenu + "/Items/Equipment/";
    
    public const string WeaponName = "Weapon";
    public const string WeaponMenu = EquipmentMenu + WeaponName;

    public const string ChestArmorName = "Chest armor";
    public const string ChestArmorMenu = EquipmentMenu + ChestArmorName;

    public const string HeadArmorName = "Head armor";
    public const string HeadArmorMenu = EquipmentMenu + HeadArmorName;
        
    #endregion

    #region Consumables

    private const string ConsumableMenu = BaseMenu + "/Items/Consumables/";

    public const string HealthPotionName = "Health potion";
    public const string HealthPotionMenu = ConsumableMenu + HealthPotionName;

    #endregion
    
    #region Inventory

    public const string InventoryName = "Inventory";
    public const string InventoryMenu = BaseMenu + "/" + InventoryName;

    #endregion
    
    #region Skill validator

    private const string ValidatorSubMenu = "Validator";
    private const string SkillValidatorMenu = BaseMenu + "/" + SkillMenu + "/" + ValidatorSubMenu + "/";
    
    public const string StatusValidatorName = "By data";
    public const string StatusValidatorMenu = SkillValidatorMenu + StatusValidatorName;

    public const string ClassValidatorName = "By Class";
    public const string ClassValidatorMenu = SkillValidatorMenu + ClassValidatorName;

    #endregion
}
