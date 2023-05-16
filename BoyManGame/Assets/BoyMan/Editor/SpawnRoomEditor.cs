#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SpawnRoom))]
public class SpawnRoomEditor : Editor
{
    // Override the default Inspector GUI
    public override void OnInspectorGUI()
    {
        // Render the default Inspector GUI elements
        base.OnInspectorGUI();

        // Get the target SpawnRoom object being edited
        SpawnRoom spawnRoom = (SpawnRoom)target;

        // Calculate the total spawn chance for all objects
        float totalSpawnChance = 0f;
        for (int i = 0; i < spawnRoom.objectsToSpawn.Length; i++)
        {
            totalSpawnChance += spawnRoom.spawnChances[i];
        }

        // Add some space in the Inspector layout
        EditorGUILayout.Space();

        // Render a slider for each object's spawn chance
        for (int i = 0; i < spawnRoom.objectsToSpawn.Length; i++)
        {
            // Calculate the percentage chance for the object
            float percentageChance = spawnRoom.spawnChances[i] / totalSpawnChance * 100;

            // Begin checking for changes in the GUI
            EditorGUI.BeginChangeCheck();

            // Define the layout rectangles for the label, slider, and value fields
            Rect sliderRect = EditorGUILayout.GetControlRect();
            Rect labelRect = new Rect(sliderRect.x, sliderRect.y, 200f, sliderRect.height);
            Rect valueRect = new Rect(sliderRect.x + 200f, sliderRect.y, sliderRect.width - 200f, sliderRect.height);

            // Render the object name label
            EditorGUI.LabelField(labelRect, spawnRoom.objectsToSpawn[i].name);

            // Render the slider for the percentage chance
            percentageChance = EditorGUI.Slider(valueRect, percentageChance, 0f, 100f);

            // If the slider value has changed, update the spawn chance and trigger spawn chance update
            if (EditorGUI.EndChangeCheck())
            {
                spawnRoom.spawnChances[i] = percentageChance / 100 * totalSpawnChance;
                spawnRoom.UpdateSpawnChances();
            }
        }

        // If there are any changes in the GUI, mark the target as dirty to save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif