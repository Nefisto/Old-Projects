using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.PlayableCharacterName, menuName = Nomenclature.PlayableCharacterMenu, order = -10)]
public partial class PlayableCharacterData : ActorData
{
    public override int Level => GetBaseStatus().level;
    protected override ActorData CreateNewInstance => CreateInstance<PlayableCharacterData>();

    [TabGroup("Data tab", "Status")]
    [Title("Status")]
    [HideLabel]
    [SerializeField]
    private Status baseStatus;

    public override Status GetBaseStatus()
        => baseStatus;

    public override object Clone()
    {
        var playableCharacterClone = (PlayableCharacterData)base.Clone();

        playableCharacterClone.baseStatus = baseStatus;

        return playableCharacterClone;
    }
}