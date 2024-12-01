using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Portal))]
public class PortalInspector : Editor
{
    public override void OnInspectorGUI()
    {
        // Reference the Portal script attached to the GameObject
        Portal portal = (Portal)target;

        // Draw the default inspector (shows all fields of Portal script)
        DrawDefaultInspector();

        // Conditionally show _nextLevel based on _isLastPortal value
        if (portal._isLastPortal)
        {
            // If it's the last portal, do not show _nextLevel field
            portal._nextLevel = null; // Ensure it's set to null, as it's not needed
        }
        else
        {
            // Otherwise, show the _nextLevel field in the Inspector
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_nextLevel"));
        }

        // Apply modified properties to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}
