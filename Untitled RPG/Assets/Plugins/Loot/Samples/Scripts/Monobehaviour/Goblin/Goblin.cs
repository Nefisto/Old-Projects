using UnityEngine;

namespace Sample
{
    public class Goblin : MonoBehaviour
    {
        [Header("Status")]
        public int strength = 1;

        public int stamina = 1;
        public int agility = 1;

        public void Setup (GoblinData data)
        {
            strength = data.strength;
            stamina = data.stamina;
            agility = data.agility;
        }
    }
}