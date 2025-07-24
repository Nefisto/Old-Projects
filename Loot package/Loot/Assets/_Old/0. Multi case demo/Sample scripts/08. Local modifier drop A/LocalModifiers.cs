using System.Linq;
using UnityEngine;

namespace Sample
{
    /*
     * Here I'll try to show 2 cases in 1, we will work with 3 scripts in this case
     *  1. LocalModifier: This will apply some change to a DropTable that will be common to next two scripts
     *  2. AngryModifier: Will get the common table and apply custom changes based on how Angry the "monster" is (TemporaryModifierA)
     *  3. ExpertiseModifier: Will get the common table and apply custom modifier based on "player" expertise in something (TemporaryModifierB)
     */
    public class LocalModifiers : MonoBehaviour
    {
        [Header("Settings")]
        public CaseData caseData;
        
        private void Start()
        {
            // Lazy
            var table = caseData.dropTable;
        
            // DOC: unique ways to add rules
            // This rule will drop 2x for each drop of CURRENCY type
            table.OnLocalModify += ctx =>
            {
                var drop = ctx.CurrentDrop;
        
                if (!(drop.Entry is Currency))
                    return;
        
                drop.AmountRange *= 2;
            };
            
            // If this table have 2 or more card, all MISC type items will get a bonus of flat 10% in drop chance
            table.OnLocalModified += ctx => // We are using ModifIED here instead of ModifY, cause we don't want to count amount of cards for each item that we go though 
            {
                var modifiedDrops = ctx.ModifiedDrops;
        
                var amountOfCards = modifiedDrops
                    .Count(d => d.Entry is Misc m && m.name.ToUpper().Contains("CARD"));
                
                if (amountOfCards < 2)
                {
                    return;
                }
        
                var miscList = modifiedDrops
                    .Where(d => d.Entry is Misc);
                
                foreach (var drop in miscList)
                {
                    drop.Percentage += 10f;
                }
            };
        
            // All CURRENCY that have COIN in name will be guaranteed to drop
            table.OnLocalModify += ctx =>
            {
                var drop = ctx.CurrentDrop;
        
                if (!(drop.Entry is Currency c))
                {
                    return;
                }
        
                if (!c.name.ToUpper().Contains("COIN"))
                {
                    return;
                }
        
                drop.IsGuaranteed = true;
            };
        }
    }
}