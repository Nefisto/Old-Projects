using Sirenix.OdinInspector;
using UnityEngine;

public class Comparer : MonoBehaviour
{
    public TestA a, b;
    
    [Button]
    private void Comp ()
    {
        Debug.Log($"{a == b}");
        Debug.Log($"{a.Equals(b)}");
    }
}