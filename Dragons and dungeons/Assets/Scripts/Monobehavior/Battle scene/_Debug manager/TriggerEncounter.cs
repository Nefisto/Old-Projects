#if UNITY_EDITOR
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DebugPanel
{
    public class TriggerEncounter : MonoBehaviour
    {
        public bool triggerOnStart = true;

        [Title("Control")]
        public AssetReference player;
        public AssetReference enemyGroup;

        private AsyncOperationHandle loadHandle;
        
        private IEnumerator Start()
        {
            yield return LoadPlayer();

            yield return LoadedEnemyGroup();
            
            if (!triggerOnStart)
                yield break;

            SpawnActors();
        }

        private object LoadedEnemyGroup()
        {
            var enemyHandle = enemyGroup.LoadAssetAsync<EnemyGroup>();
            enemyHandle.Completed += LoadedEnemyGroup;

            return enemyHandle;
        }

        private void LoadedEnemyGroup (AsyncOperationHandle<EnemyGroup> obj)
        {
            if (obj.Status != AsyncOperationStatus.Succeeded)
                Debug.Log($"Enemy group cant be loaded");
        }

        private object LoadPlayer()
        {
            var playerHandle = player.LoadAssetAsync<Player>();
            playerHandle.Completed += LoadedPlayer;

            return playerHandle;
        }

        private void LoadedPlayer (AsyncOperationHandle<Player> obj)
        {
            if (obj.Status != AsyncOperationStatus.Succeeded)
                Debug.Log($"Player cant be loaded");
        }

        public void SpawnActors()
        {
            var context = new BattleEncounterContext()
            {
                player = player.Asset as Player,
                enemyGroup = enemyGroup.Asset as EnemyGroup
            };
            
            GameEvents.Battle.RaiseSetupBattle(context);
        }
    }
}
#endif
