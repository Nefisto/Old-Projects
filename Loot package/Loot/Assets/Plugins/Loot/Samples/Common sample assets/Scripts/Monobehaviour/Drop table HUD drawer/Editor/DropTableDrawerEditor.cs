#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OldSample.Utilities.Editor
{
    [CustomEditor(typeof(DropTableDrawer))]
    public class DropTableDrawerEditor : UnityEditor.Editor
    {
        private DropTableDrawer Target => (DropTableDrawer)target;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying)
                GUI.enabled = false;

            if (GUILayout.Button("Redraw"))
                Target.DrawDropTable(Target.originalTable);
        }
    }
}
#endif