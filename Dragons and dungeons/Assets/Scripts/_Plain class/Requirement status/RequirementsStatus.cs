using System;
using Sirenix.OdinInspector;

[Serializable]
public class RequirementsStatus
{
    [MinValue(1), MaxValue(8)]
    public int level;

    [HideLabel]
    public CharacterAttributes attributes;
}