using Loot;
using UnityEngine;

namespace Sample
{
    /// <summary>
    ///     Just to simplify the code inside each sample
    /// </summary>
    public abstract class BaseCase : MonoBehaviour
    {
        [Header("Settings")]
        [Multiline(10)]
        [SerializeField]
        private string sampleInfo = "";

        [SerializeField]
        protected DropTable dropTable;

        public void OnHover()
        {
            if (!SampleSettings.CanHover)
                return;

            UpdateDropList();
            UpdateSampleInfo(sampleInfo);
        }

        private void UpdateSampleInfo (string info)
            => GameEvents.RaiseUpdateSampleInfo(info);

        private void UpdateDropList()
            => GameEvents.RaiseUpdateDropTableDrawer(dropTable);
    }
}