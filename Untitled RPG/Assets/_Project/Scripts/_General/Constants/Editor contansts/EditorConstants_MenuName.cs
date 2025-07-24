public static partial class EditorConstants
{
    public static class MenuAssets
    {
        public const string GAME_MENU = "Untitled RPG/";

        public const string ENEMY_MENU = GAME_MENU + "Enemy/";
        public const string ENEMY_BRAIN = ENEMY_MENU + "Enemy AI/";

        public const string EQUIPMENT_MENU = GAME_MENU + "Equipment/";

        public const string LOCATION_MODIFIERS = GAME_MENU + "Location modifier/";

        private const string SKILLS = GAME_MENU + "Skills/";

        public const string SERVICES = GAME_MENU + "Services/";
        public const string DATABASE = GAME_MENU + "Database/";

        private const string STATUS_EFFECT = GAME_MENU + "Status effect/";
        public const string BUFF_EFFECT = STATUS_EFFECT + "Buffs/";
        public const string DOT_EFFECT = STATUS_EFFECT + "DoTs/";
        public const string DEBILITATOR_EFFECT = STATUS_EFFECT + "Debilitator/";
        public const string ATTRIBUTE_MODIFIER = STATUS_EFFECT + "Attribute modifier/";

        public const string ACTIVE_SKILLS = SKILLS + "Active skills/";
        public const string BASIC_ACTIONS_SKILLS = ACTIVE_SKILLS + "Basic actions/";
        public const string ENEMY_SKILLS = SKILLS + "Enemy skills/";
        public const string PASSIVE_SKILLS = SKILLS + "Passive skill/";
        public const string JOB_SKILLS = SKILLS + "Job skill/";

        public const string CLICK_BEHAVIOR = GAME_MENU + "Click operations/";

        public const string GAME_JOB = GAME_MENU + "Classes/";

        public const string SPECIAL_RESOURCES = GAME_MENU + "Special resources/";

        private const string CHARGE_SKILLS = SKILLS + "Charge abilities/";

        private const string LEVEL_CHARGE_SKILL = CHARGE_SKILLS + "Level charge/";
        public const string INSTANT_CHARGE_SKILLS = LEVEL_CHARGE_SKILL + "Instant/";
        public const string PERSISTENT_CHARGE_SKILLS = LEVEL_CHARGE_SKILL + "Persistent/";

        public const string SINGLE_CHARGE_SKILL = CHARGE_SKILLS + "Single charge/";
    }
}