using UnityEditor;
using UnityEngine;

public class CustomProBuilderWindow : EditorWindow
{
    [MenuItem("Tools/ProBuilder/Open Custom Window")]
    public static void OpenWindow()
    {
        GetWindow<CustomProBuilderWindow>("ProBuilder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Custom ProBuilder Window", EditorStyles.boldLabel);
    }
}
