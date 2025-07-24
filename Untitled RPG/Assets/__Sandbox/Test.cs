using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    public List<MyTestClass> myTestClasses;

    [Button]
    public void CheckThem()
    {
        var dict = new Dictionary<MyTestClass, int>(new MyTestClass.MyTestClassComparer())
        {
            { new MyTestClass { kind = MyTestEnum.One }, 0 },
            { new MyTestClass { kind = MyTestEnum.Two }, 0 },
            { new MyTestClass { kind = MyTestEnum.Tree }, 0 }
        };

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

public enum MyTestEnum
{
    One,
    Two,
    Tree
}

[Serializable]
public class MyTestClass
{
    public MyTestEnum kind;

    public class MyTestClassComparer : IEqualityComparer<MyTestClass>
    {
        public bool Equals (MyTestClass x, MyTestClass y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (ReferenceEquals(x, null))
                return false;
            if (ReferenceEquals(y, null))
                return false;
            if (x.GetType() != y.GetType())
                return false;
            return x.kind == y.kind;
        }

        public int GetHashCode (MyTestClass obj) => (int)obj.kind;
    }
}