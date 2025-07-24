using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleATB : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private RectTransform atbRectSize;

    [TitleGroup("References")]
    [SerializeField]
    private RectTransform markZero;

    [TitleGroup("References")]
    [SerializeField]
    private Transform actorsATBFolder;

    [FormerlySerializedAs("actorATBPrefab")]
    [TitleGroup("References")]
    [SerializeField]
    private ActorATBIcon actorATBIconPrefab;

    private void Start() => BattleManager.afterSetupBattleActorsListeners += SetupATB;

    private IEnumerator SetupATB (BattleSetupContext _)
    {
        Clear();
        foreach (var battleActor in ServiceLocator.BattleContext.AllBattleActor)
        {
            var instance = Instantiate(actorATBIconPrefab, actorsATBFolder, false);
            battleActor.actorATBIcon = instance;

            battleActor.onDie += _ => Destroy(instance.gameObject);

            yield return instance.Setup(new ActorATBIcon.Settings()
            {
                battleActor = battleActor,
                atbBarSize = atbRectSize.rect.width,
                markZero = markZero
            });
        }
    }

    private void Clear()
    {
        foreach (Transform child in actorsATBFolder)
            Destroy(child.gameObject);
    }
}