namespace Loot.Utilities
{
    public static class Messages
    {
        public static string DeeperThanAllowedMaxDepth =>
            $"You have made more than {LootSettings.MaxDepthLayers} iterations, this probably will end in a stack overflow. Disable this check in settings if you know what you are doing.";

        public static string DestructiveOperationsOnOriginalAssetsWarning =>
            "You are trying to make destructive operations on a non clone table, this can lead to permanent changes on your assets." +
            "Clone this table OR mark the EnableWorkOnOriginal through LootSettings or via editor in Tools->Loot->Settings";
    }
}