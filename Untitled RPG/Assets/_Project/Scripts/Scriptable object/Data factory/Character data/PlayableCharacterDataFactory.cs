using UnityEngine;

[CreateAssetMenu(fileName = "Character data", menuName = EditorConstants.MenuAssets.GAME_MENU + "Character data",
    order = 0)]
public class PlayableCharacterDataFactory : ScriptableObjectFactory<PlayableCharacterData>
{
    public override PlayableCharacterData GetInstance() => base.GetInstance();
}