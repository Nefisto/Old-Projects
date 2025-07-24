#if UNITY_EDITOR && UNITY_2020_1_OR_NEWER
using System;
using System.Collections;
using Loot.Editor;
using UnityEngine;

namespace Loot
{
    public class DebuggerRefresher : MonoBehaviour
    {
        [Multiline]
        public string description = "This component is used to tell the debugger to refresh.";

        private IEnumerator refreshRoutine;

        private void OnEnable()
        {
            refreshRoutine = Refresh();
            StartRefresh();
        }

        public event Action OnRefresh;

        public IEnumerator Refresh()
        {
            while (true)
            {
                yield return new WaitForSeconds(LootDebugger.RefreshRate);
                OnRefresh?.Invoke();
            }
        }

        public void StopRefresh()
            => StopCoroutine(refreshRoutine);

        public void StartRefresh()
            => StartCoroutine(refreshRoutine);
    }
}
#endif