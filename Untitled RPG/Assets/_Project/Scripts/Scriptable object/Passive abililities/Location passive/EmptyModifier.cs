using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Empty modifier",
    menuName = EditorConstants.MenuAssets.LOCATION_MODIFIERS + "Empty modifier")]
public class EmptyModifier : LocationModifier
{
    public override IEnumerator Register()
    {
        yield break;
    }

    public override IEnumerator Remove()
    {
        yield break;
    }

    public override string NameShowOnField => string.Empty;
}