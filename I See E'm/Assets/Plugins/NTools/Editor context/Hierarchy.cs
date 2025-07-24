using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class Hierarchy
{
    /// <summary>
    /// Sometimes parent is wrong positioned, this will un-parent all children, reset parent position and
    /// parent all children again    
    /// </summary>
    [MenuItem("GameObject/NTools/Reset position (parent only)")]
    private static void ResetParentPositionOnly()
    {
        Undo.RegisterFullObjectHierarchyUndo(Selection.activeObject, "Reset position (parent only)");
        
        // Get all children
        var selected = Selection.activeTransform;
        var children = selected
            .Cast<Transform>() // Iterating over transform will give all children
            .ToList();

        // Un-parent
        children.ForEach(child => child.parent = null);

        // Reset position
        selected.localPosition = Vector3.zero;
        
        // // Re-parent
        children.ForEach(child => child.parent = selected);
    }

    [MenuItem("GameObject/NTools/Reset position (parent only)", true)]
    private static bool ResetParentPositionOnlyValidation()
    {
        return Selection.activeObject != null && Selection.activeTransform.parent != null;
    }
}