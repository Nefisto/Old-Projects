using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class HoldingIconManager : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Transform instantFolder;

    [TitleGroup("References")]
    [SerializeField]
    private Transform permanentFolder;

    [TitleGroup("References")]
    [SerializeField]
    private HoldingIcon instantPrefab;

    [TitleGroup("References")]
    [SerializeField]
    private HoldingIcon permanentPrefab;

    public IEnumerator Setup (Skill.ChargeLevelSettings chargeSettings)
    {
        Clean();

        foreach (var settings in chargeSettings.chargeAbilities)
        {
            var (prefab, folder) = settings is InstantChargeAbility instantChargeAbility
                ? (instantPrefab, instantFolder)
                : (permanentPrefab, permanentFolder);

            var instance = Instantiate(prefab, folder, false);

            yield return instance.Setup(settings);
        }
    }

    private void Clean()
    {
        foreach (Transform child in instantFolder.transform)
            Destroy(child.gameObject);

        foreach (Transform child in permanentFolder.transform)
            Destroy(child.gameObject);
    }
}