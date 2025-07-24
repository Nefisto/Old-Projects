using System.Collections;
using System.Linq;
using NTools;
using QFSW.QC;
using QFSW.QC.Suggestors.Tags;

// ReSharper disable UnusedType.Global

[CommandPrefix(CommandConstants.BASE_PATH)]
public static class Commands
{
    [Command("Reset-board")]
    private static void ResetBoard()
    {
        new NTask(Routine());

        IEnumerator Routine()
        {
            foreach (var blockData in CommonOperations
                         .GetAllBlocksOnCurrentRoom()
                         .Where(bd => bd.HasUnitOnIt))
            {
                yield return blockData.ApplyDamage(new BlockData.ApplyDamageSettings()
                {
                    damage = 50,
                    showDamageTest = false
                });
            }

            yield return CommonOperations.ProcessDeathAnimation();
        }
    }

    [Command("Add-coins",
        description: "Place a negative value to remove coins. Leave empty to add 5 coins.")]
    private static void AddCoins ([Suggestions("Leave empty", 5, -5)] int amount = 5)
        => ServiceLocator.GameContext.PlayerData.Coins.Value += amount;
}