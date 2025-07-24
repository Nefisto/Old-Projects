using QFSW.QC;

public partial class RewardController
{
    [Command(CommandConstants.BASE_PATH + "Trigger-reward")]
    private void TriggerRewardCommand() => StartCoroutine(RewardScreenHandle());
}