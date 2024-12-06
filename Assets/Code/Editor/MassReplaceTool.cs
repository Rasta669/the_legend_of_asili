using UnityEditor;
using UnityEngine;

public class MassReplaceTool : EditorWindow
{
    private GameObject replacementPrefab;
    private Vector3 positionOffset = Vector3.zero;
    private Vector3 rotationOffset = Vector3.zero;

    // New scale-related fields
    private Vector3 scaleMultiplier = Vector3.one;
    private bool randomizeScale = false;
    private float randomScaleMin = 0.8f;
    private float randomScaleMax = 1.5f;

    [MenuItem("Tools/Mass Replace Tool")]
    public static void ShowWindow()
    {
        GetWindow<MassReplaceTool>("Mass Replace Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Mass Replace Tool", EditorStyles.boldLabel);

        // Replacement Prefab Field
        replacementPrefab = (GameObject)EditorGUILayout.ObjectField("Replacement Prefab", replacementPrefab, typeof(GameObject), false);

        GUILayout.Space(10);

        // Position and Rotation Offset
        positionOffset = EditorGUILayout.Vector3Field("Position Offset", positionOffset);
        rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", rotationOffset);

        GUILayout.Space(10);

        // Scale settings
        GUILayout.Label("Scale Settings", EditorStyles.boldLabel);
        scaleMultiplier = EditorGUILayout.Vector3Field("Scale Multiplier", scaleMultiplier);

        randomizeScale = EditorGUILayout.Toggle("Randomize Scale", randomizeScale);
        if (randomizeScale)
        {
            randomScaleMin = EditorGUILayout.FloatField("Random Scale Min", randomScaleMin);
            randomScaleMax = EditorGUILayout.FloatField("Random Scale Max", randomScaleMax);
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Replace Selected Objects"))
        {
            if (replacementPrefab == null)
            {
                Debug.LogError("Please assign a replacement prefab before proceeding.");
                return;
            }

            ReplaceSelectedObjects();
        }
    }

    private void ReplaceSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected. Please select objects in the scene to replace.");
            return;
        }

        // Register the entire operation for Undo
        Undo.RegisterCompleteObjectUndo(selectedObjects, "Mass Replace Objects");

        foreach (GameObject obj in selectedObjects)
        {
            // Save original object's transform
            Vector3 originalPosition = obj.transform.position;
            Quaternion originalRotation = obj.transform.rotation;
            Vector3 originalScale = obj.transform.localScale;

            // Calculate new transform
            Vector3 newPosition = originalPosition + positionOffset;
            Quaternion newRotation = originalRotation * Quaternion.Euler(rotationOffset);

            // Instantiate replacement prefab and register it for Undo
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(replacementPrefab, obj.scene);
            if (newObject != null)
            {
                Undo.RegisterCreatedObjectUndo(newObject, "Replace Object");

                // Apply new position and rotation
                newObject.transform.position = newPosition;
                newObject.transform.rotation = newRotation;

                // Apply scale
                if (randomizeScale)
                {
                    float randomUniformScale = Random.Range(randomScaleMin, randomScaleMax);
                    newObject.transform.localScale = originalScale * randomUniformScale;
                }
                else
                {
                    newObject.transform.localScale = Vector3.Scale(originalScale, scaleMultiplier);
                }
            }

            // Destroy the original object and register it for Undo
            Undo.DestroyObjectImmediate(obj);
        }

        Debug.Log($"Replaced {selectedObjects.Length} objects with '{replacementPrefab.name}'.");
    }
}
