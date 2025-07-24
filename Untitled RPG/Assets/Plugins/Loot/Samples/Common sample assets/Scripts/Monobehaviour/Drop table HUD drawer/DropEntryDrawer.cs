using Loot;
using Sample;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OldSample.Utilities
{
    public class DropEntryDrawer : MonoBehaviour
    {
        public Image entryIcon;
        public TextMeshProUGUI entryName;
        public TextMeshProUGUI entryMinimumAmount;
        public TextMeshProUGUI entryMaxAmount;
        public TextMeshProUGUI entryPercentage;

        public void FillEntry (Drop drop)
        {
            FillName(drop);
            FillIcon(drop);
            FillMinimumAmount(drop);
            FillMaxAmount(drop);
            FillPercentage(drop);
        }

        private void FillPercentage (Drop drop)
            => entryPercentage.text = drop.IsDisabled
                ? "Locked"
                : $"{drop.GetPercentage():F2}";

        private void FillMaxAmount (Drop drop)
            => entryMaxAmount.text = drop.AmountRange.y.ToString();

        private void FillMinimumAmount (Drop drop)
            => entryMinimumAmount.text = drop.AmountRange.x.ToString();

        private void FillIcon (Drop drop)
        {
            if (drop.Entry == null)
                return;

            if (drop.Entry is Item item)
                entryIcon.sprite = item.icon;
            else if (drop.Entry is DropTable table)
                entryIcon.sprite = table.Icon;
        }

        private void FillName (Drop drop)
            => entryName.text = drop.Entry == null ? "Empty" : drop.Entry.name;
    }
}