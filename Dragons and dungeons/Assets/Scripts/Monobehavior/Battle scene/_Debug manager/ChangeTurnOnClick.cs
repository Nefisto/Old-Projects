#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DebugPanel
{
    public class ChangeTurnOnClick : MonoBehaviour
    {
        [Title("Actors")]
        public RuntimeSet battleActorGroup;

        private void Start()
            => BattleManager.Instance.OnSpawnActors += ApplyChangeTurnOnActorClick;

        private void ApplyChangeTurnOnActorClick()
        {
            foreach (var battleActor in battleActorGroup.items.Select(r => r.GetComponent<BattleActor>()))
            {
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerClick
                };

                if (battleActor is EnemyBattleActor enemy)
                {
                    // Enemies would show message when right click
                    entry.callback.AddListener(data =>
                    {
                        var pointerData = (PointerEventData)data;
                
                        if (pointerData.button == PointerEventData.InputButton.Left)
                            ChangeTurn(battleActor);
                        else
                            StartCoroutine(enemy.ShowBeginningBattleMessage());
                    });
                }
                else
                    entry.callback.AddListener(_ => ChangeTurn(battleActor));
                 
                battleActor.GetComponent<EventTrigger>().triggers.Add(entry);
            }
        }
        
        private void ChangeTurn (BattleActor actor)
        {
            var newTurnOrder = new List<BattleActor> { actor };
            var otherActors = BattleManager
                .Instance
                .GetAllTargetButMe(actor)
                .OrderByDescending(a => a.Data.GetBaseStatus().level);
            
            newTurnOrder.AddRange(otherActors);

            BattleManager.Instance.ManuallyInsertOrder(newTurnOrder);

            BattleManager.Instance.EndTurnOfCurrentActor();
        }
    }
}
#endif