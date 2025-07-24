using System.Collections;

public class NullBattleResultHUD : IBattleResultHUD
{
    public void Setup (BattleSetupContext ctx) { }

    public IEnumerator Run (BattleResultData resultData)
    {
        yield return null;
    }
}