using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

// TODO: SAVE/LOAD this script - It's temporary using the actual assets, be careful when change values from characters
[CreateAssetMenu(fileName = Nomenclature.PlayerStatusName, menuName = Nomenclature.PlayerStatusMenu, order = 0)]
public class Player : ScriptableObject
{
    [Title("Characters")]
    [SerializeField]
    private PlayableCharacterData characterA;

    [SerializeField]
    private PlayableCharacterData characterB;

    [SerializeField]
    private PlayableCharacterData characterC;

    public PlayableCharacterData GetPlayableCharacterAtIndex (int index)
        => GetPlayableCharactersEnumerator()
            .Skip(index)
            .First();

    private IEnumerable<PlayableCharacterData> GetPlayableCharactersEnumerator()
    {
        yield return characterA;
        yield return characterB;
        yield return characterC;
    }
}