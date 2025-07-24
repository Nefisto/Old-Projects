using Loot;
using UnityEngine;

namespace Sample
{
    public class CaseData : ScriptableObject
    {
        [Multiline]
        public string description;

        public DropTable dropTable;
    }
}