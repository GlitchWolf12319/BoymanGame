using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventCollider))]
public class EventColliderEditor : Editor
{
    // Override the default Inspector GUI
    public override void OnInspectorGUI()
    {
        // Render the default Inspector GUI elements
        base.OnInspectorGUI();

        // Get the target EventCollider object being edited
        EventCollider eventCollider = (EventCollider)target;

        // Calculate the total spawn chance for all prefabs
        float totalSpawnChance = 0f;
        for (int i = 0; i < eventCollider.prefabsToInstantiate.Length; i++)
        {
            totalSpawnChance += eventCollider.spawnChances[i];
        }

        // Add some space in the Inspector layout
        EditorGUILayout.Space();

        // Render a slider for each prefab's spawn chance
        for (int i = 0; i < eventCollider.prefabsToInstantiate.Length; i++)
        {
            // Calculate the percentage chance for the prefab
            float percentageChance = eventCollider.spawnChances[i] / totalSpawnChance * 100;

            // Begin checking for changes in the GUI
            EditorGUI.BeginChangeCheck();

            // Render the slider for adjusting the spawn chance
            percentageChance = EditorGUILayout.Slider("Spawn Chance for " + eventCollider.prefabsToInstantiate[i].name + " (%):", percentageChance, 0f, 100f);

            // If the slider value has changed, update the spawn chance
            if (EditorGUI.EndChangeCheck())
            {
                eventCollider.spawnChances[i] = percentageChance / 100 * totalSpawnChance;
            }
        }

        // If there are any changes in the GUI, mark the target as dirty to save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}