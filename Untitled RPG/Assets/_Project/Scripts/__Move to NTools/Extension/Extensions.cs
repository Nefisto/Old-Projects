using UnityEngine;

public static partial class Extensions
{
    public static void DestroyChildren (this Transform transform)
    {
        foreach (Transform child in transform)
            Object.Destroy(child.gameObject);
    }

    public static T GetInstance<T> (this T so) where T : ScriptableObject => Object.Instantiate(so);
}