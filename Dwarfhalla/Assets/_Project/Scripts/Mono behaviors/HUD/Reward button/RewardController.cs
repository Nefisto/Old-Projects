using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using QFSW.QC;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class RewardController : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private List<SummonCard> possibleRewards;

    [TitleGroup("References")]
    [SerializeField]
    private RewardScreen rewardScreen;

    private IEnumerator RewardScreenHandle()
    {
        yield return rewardScreen.RewardRoutine(new RewardScreen.RewardContext
        {
            rewards = possibleRewards
                .Shuffle()
                .Take(3)
                .ToList()
        });
    }
}