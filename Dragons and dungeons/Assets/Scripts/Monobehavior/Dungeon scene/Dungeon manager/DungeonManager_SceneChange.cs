using System;
using System.Collections;
using UnityEngine.SceneManagement;

public partial class DungeonManager
{
    public event Action OnChangeToBattleScene;

    private void RegisterOnNodeEncounterEvents()
        => dungeonNodes.ForEach(x => x.OnEncounter += ChangeToBattleScene);

    private void ChangeToBattleScene (BattleEncounterContext ctx)
    {
        ctx.player = (Player)player.Asset;

        StartCoroutine(_Encounter());

        IEnumerator _Encounter()
        {
            // Fade in
            yield return fadeBackground.FadeInRoutine();
            
            OnChangeToBattleScene?.Invoke();
            
            // Load battle scene
            var sceneChangeHandle = battleScene.LoadSceneAsync(LoadSceneMode.Additive);
            yield return sceneChangeHandle;

            ctx.ReturnToDungeonMethod += ResumeDungeon;
            
            // Trigger event
            GameEvents.Battle.RaiseSetupBattle(ctx);
        }
    }

    private void ResumeDungeon()
    {
        StartCoroutine(_Resume());
        
        IEnumerator _Resume()
        {
            yield return battleScene.UnLoadScene();

            GameEvents.DungeonMap.RaisePreToResumeDungeon();
            yield return fadeBackground.FadeOutRoutine();
        
            GameEvents.DungeonMap.RaiseResumeDungeon();
        }
    }
}