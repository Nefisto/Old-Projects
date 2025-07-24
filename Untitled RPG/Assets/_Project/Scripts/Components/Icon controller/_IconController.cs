using System.Collections;
using UnityEngine;

public abstract class IconController : MonoBehaviour
{
    public abstract IEnumerator CreateIcon (StatusEffectData instance);
}