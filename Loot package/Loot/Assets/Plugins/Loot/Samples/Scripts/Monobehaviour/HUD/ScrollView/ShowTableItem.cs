using Sample;
using TMPro;
using UnityEngine;

namespace OldSample
{
    public class ShowTableItem : MonoBehaviour
    {
        public TextMeshProUGUI refToAmount;
        public TextMeshProUGUI refToName;
        public TextMeshProUGUI refToPercentage;

        #region API

        public void Fill (HUDScrollVIewItem item)
        {
            refToName.text = item.Name;
            refToAmount.text = $"{item.Amount.min} ~ {item.Amount.max}";
            refToPercentage.text = $"% {item.Percentage:F2}";
        }

        #endregion
    }
}