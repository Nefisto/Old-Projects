using Sample;
using Sample.Utilities;
using TMPro;
using UnityEngine;

namespace OldSample.Utilities
{
    public class CommonGUIController : MonoBehaviour
    {
        [Header("Control")]
        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private TextMeshProUGUI info;

        [SerializeField]
        private GameObject globalSettingsFolder;

        [SerializeField]
        private DropTableDrawer dropTableDrawer;

        private void OnEnable()
            => GameEvents.OnUpdateSampleInfo += UpdateSampleInfo;

        private void OnDisable()
            => GameEvents.OnUpdateSampleInfo -= UpdateSampleInfo;

        public void Setup (GUISetupContext ctx)
        {
            UpdateSampleTitle(ctx.SampleTitle);
            UpdateSampleInfo(ctx.SampleInfo);

            globalSettingsFolder.SetActive(ctx.ShouldShowGlobalPanel);

            if (ctx.ShouldShownTreatTableAsDropOption)
                dropTableDrawer.EnableTreatTableAsDrop();
            else
                dropTableDrawer.DisableTreatTableAsDrop();
        }

        public void UpdateSampleInfo (string newInfo)
            => info.text = newInfo;

        public void UpdateSampleTitle (string newTitle)
            => title.text = newTitle;
    }
}