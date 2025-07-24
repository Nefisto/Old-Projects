using Sirenix.OdinInspector;
using UnityEngine;

public partial class GameConstantsSO
{
    [field: TitleGroup("Experience Table")]
    [field: HideLabel]
    [field: SerializeField]
    public ExperienceTable ExperienceTable { get; private set; }
}