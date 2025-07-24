using Sample;
using UnityEngine;

namespace OldSample
{
    public class Log : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField]
        private GameObject entryPrefab;

        [Header("Control")]
        [SerializeField]
        private Transform contentFolder;

        private void OnEnable()
            => GameEvents.OnUpdateLog += CreateEntry;

        private void OnDisable()
            => GameEvents.OnUpdateLog -= CreateEntry;

        public void ClearLog()
        {
            foreach (Transform child in contentFolder)
                Destroy(child.gameObject);
        }

        private void CreateEntry (LogEntryContext ctx)
        {
            var instance = Instantiate(entryPrefab, contentFolder, false)
                .GetComponent<LogEntry>();

            instance.Log(ctx);
        }
    }
}