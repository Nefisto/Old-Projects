using Loot;
using Sample;
using UnityEngine;
using UnityEngine.UI;

namespace OldSample.Utilities
{
    public class DropTableDrawer : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("This entries will be drawn at start. If null will only clean")]
        public DropTable originalTable;

        [Header("Control")]
        [Tooltip("Content parent")]
        [SerializeField]
        private GameObject contentFolder;

        [Tooltip("Prefab for each drop entry")]
        [SerializeField]
        private GameObject dropMenuEntry;

        [Tooltip("Toggle to decide if tables should be treated as drop or not")]
        [SerializeField]
        private Toggle shouldTreatTableAsDrop;

        private DropTable cachedTable;

        private bool HasCacheTable => cachedTable != null;

        private void Start()
        {
            if (originalTable)
                DrawDropTable(originalTable);
            else
                CleanEntries();
        }

        private void OnEnable()
            => GameEvents.OnUpdateDropTableDrawer += DrawDropTable;

        private void OnDisable()
            => GameEvents.OnUpdateDropTableDrawer -= DrawDropTable;

        public void DrawDropTable (DropTable table)
        {
            CleanEntries();
            DrawEntries(table);
            cachedTable = table;
        }

        public void DrawCachedTable()
        {
            if (!HasCacheTable)
            {
                Debug.LogWarning("Cached table is null");
                return;
            }

            CleanEntries();
            DrawEntries(cachedTable);
        }

        public void EnableTreatTableAsDrop()
            => shouldTreatTableAsDrop.gameObject.SetActive(true);

        public void DisableTreatTableAsDrop()
            => shouldTreatTableAsDrop.gameObject.SetActive(false);

        private void CleanEntries()
        {
            foreach (Transform child in contentFolder.transform)
                Destroy(child.gameObject);
        }

        private void DrawEntries (DropTable entries)
        {
            foreach (var entry in entries)
            {
                var entryDrawer = Instantiate(dropMenuEntry, contentFolder.transform, false)
                    .GetComponent<DropEntryDrawer>();

                entryDrawer.FillEntry(entry);
            }
        }
    }
}