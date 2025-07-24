using UnityEngine;

namespace Sample
{
    public class GoblinData : CaseData
    {
        [Header("Status")]
        public int strength = 1;

        public int stamina = 1;
        public int agility = 1;

        [Space]
        public GameObject prefab;
    }
}