using System.Collections;
using UnityEngine;

public abstract class ChargeMode : MonoBehaviour
{
    public abstract IEnumerator Setup (Settings settings);

    public class Settings { }
}