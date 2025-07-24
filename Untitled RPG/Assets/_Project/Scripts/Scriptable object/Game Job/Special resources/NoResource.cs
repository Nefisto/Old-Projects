using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "No resource", menuName = EditorConstants.MenuAssets.SPECIAL_RESOURCES + "No resource")]
public class NoResource : SpecialResource
{
    public override IEnumerator Setup (SetupSettings settings)
    {
        settings.gradientBar.gameObject.SetActive(false);
        yield break;
    }
}