using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Oto : MonoBehaviour
{
    public List<MyTestClass> myTestClasses;

    [Button]
    public void CheckThem()
    {
        var dict = new Dictionary<MyTestClass, int>();
        foreach (var test in myTestClasses)
        {
            if (!dict.ContainsKey(test))
            {
                dict.Add(test, 0);
                continue;
            }

            dict[test]++;
        }
    }
}