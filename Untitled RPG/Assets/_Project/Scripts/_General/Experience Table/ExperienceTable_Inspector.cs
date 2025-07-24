using System.Linq;
using UnityEngine;

public partial class ExperienceTable
{
    private string ToNextLevelInfoBox() => CreateMessageFromCurve(toNextLevel);

    private string TotalInfoBox() => CreateMessageFromCurve(Table);

    private string CreateMessageFromCurve (AnimationCurve curve)
        => Enumerable
            .Range(1, 31)
            .Select(index => (index, (int)curve.Evaluate(index)))
            .Aggregate(string.Empty,
                (message, keyframe) => message + $"Lvl:{keyframe.index:00}\t\t{keyframe.Item2}\n");

    private void RefreshTable()
    {
        var amountOfLevels = (int)toNextLevel.keys[toNextLevel.length - 1].time;

        var keyframes = new Keyframe[amountOfLevels + 1];
        keyframes[0] = new Keyframe(0, 0)
        {
            // Used to simulated a constant on both tangents
            inTangent = float.PositiveInfinity,
            outTangent = float.PositiveInfinity
        };

        float totalExpToNextLevel = 0;
        for (var i = 0; i < amountOfLevels; i++)
        {
            totalExpToNextLevel += (int)toNextLevel.Evaluate(i);
            keyframes[i] = new Keyframe(i + 1, totalExpToNextLevel)
            {
                inTangent = float.PositiveInfinity,
                outTangent = float.PositiveInfinity
            };
        }

        Table.keys = keyframes;
    }
}