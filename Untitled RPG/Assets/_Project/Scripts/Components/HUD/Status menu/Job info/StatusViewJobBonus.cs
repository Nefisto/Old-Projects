using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class StatusViewJobBonus : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private StatusViewJobBonusSector strengthSector;

    [TitleGroup("References")]
    [SerializeField]
    private StatusViewJobBonusSector dexteritySector;

    [TitleGroup("References")]
    [SerializeField]
    private StatusViewJobBonusSector vitalitySector;

    [TitleGroup("References")]
    [SerializeField]
    private StatusViewJobBonusSector intelligenceSector;

    public IEnumerator Setup (PlayableCharacterData characterData)
    {
        yield return strengthSector.Setup(characterData.Job.PotentialBonus.Strength);
        yield return dexteritySector.Setup(characterData.Job.PotentialBonus.Dexterity);
        yield return vitalitySector.Setup(characterData.Job.PotentialBonus.Vitality);
        yield return intelligenceSector.Setup(characterData.Job.PotentialBonus.Intelligence);
    }
}