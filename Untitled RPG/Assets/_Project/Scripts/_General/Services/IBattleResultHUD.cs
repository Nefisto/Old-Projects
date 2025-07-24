using System.Collections;

public interface IBattleResultHUD
{
    public void Setup (BattleSetupContext ctx);

    public IEnumerator Run (BattleResultData resultData);
}