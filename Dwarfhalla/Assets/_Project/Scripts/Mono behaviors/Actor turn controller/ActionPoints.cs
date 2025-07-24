using System;
using System.Collections;
using Sirenix.OdinInspector;

[Serializable]
public class ActionPoints
{
    [ReadOnly]
    [ShowInInspector]
    public int CurrentPoints { get; set; }

    public int MaxPoints { get; set; } = 2;

    public IEnumerator Setup()
    {
        CurrentPoints = MaxPoints;
        yield break;
    }

    public void Recharge()
    {
        if (CurrentPoints > MaxPoints)
            return;

        if (CurrentPoints == MaxPoints)
        {
            CurrentPoints = MaxPoints + 1;
            return;
        }

        CurrentPoints = MaxPoints;
    }
}