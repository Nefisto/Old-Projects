using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.StatusChangeName, menuName = Nomenclature.StatusChangeMenu)]
public class SkillPassiveStatusChange : SkillPassive, IStatusChange
{
    [Title("Status to change")]
    [HideLabel]
    [SerializeField]
    private CharacterAttributes attributes;
    
    public Status ChangeStatus (StatusChangeContext ctx)
    {
        return Status.ConstructWithAttributes(attributes);    
    }
}