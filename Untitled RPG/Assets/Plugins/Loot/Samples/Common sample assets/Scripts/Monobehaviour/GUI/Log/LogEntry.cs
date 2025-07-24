using Sample;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OldSample
{
    public class LogEntry : MonoBehaviour
    {
        [Header("Control")]
        [SerializeField]
        private Image icon;

        [SerializeField]
        private TextMeshProUGUI entryNameLabel;

        [SerializeField]
        private TextMeshProUGUI amountLabel;

        public void Log (LogEntryContext ctx)
        {
            EnableEntry(ctx);
            CopyEntry(ctx);
        }

        private void CopyEntry (LogEntryContext ctx)
        {
            icon.sprite = ctx.Icon;
            entryNameLabel.text = ctx.Label;
            amountLabel.text = ctx.Amount.ToString().PadLeft(2, '0');
        }

        private void EnableEntry (LogEntryContext ctx)
        {
            var hasIcon = ctx.Icon;
            var hasName = !string.IsNullOrWhiteSpace(ctx.Label);
            var hasAmount = ctx.Amount != 0;

            icon.gameObject.SetActive(hasIcon);
            entryNameLabel.gameObject.SetActive(hasName);
            amountLabel.gameObject.SetActive(hasAmount);
        }
    }
}