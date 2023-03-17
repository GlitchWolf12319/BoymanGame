#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SpawnRoom))]
public class SpawnRoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpawnRoom spawnRoom = (SpawnRoom)target;

        float totalSpawnChance = 0f;
        for (int i = 0; i < spawnRoom.objectsToSpawn.Length; i++)
        {
            totalSpawnChance += spawnRoom.spawnChances[i];
        }

        EditorGUILayout.Space();

        for (int i = 0; i < spawnRoom.objectsToSpawn.Length; i++)
        {
            float percentageChance = spawnRoom.spawnChances[i] / totalSpawnChance * 100;

            EditorGUI.BeginChangeCheck();

            Rect sliderRect = EditorGUILayout.GetControlRect();
            Rect labelRect = new Rect(sliderRect.x, sliderRect.y, 200f, sliderRect.height);
            Rect valueRect = new Rect(sliderRect.x + 200f, sliderRect.y, sliderRect.width - 200f, sliderRect.height);

            EditorGUI.LabelField(labelRect, spawnRoom.objectsToSpawn[i].name);
            percentageChance = EditorGUI.Slider(valueRect, percentageChance, 0f, 100f);

            if (EditorGUI.EndChangeCheck())
            {
                spawnRoom.spawnChances[i] = percentageChance / 100 * totalSpawnChance;
                spawnRoom.UpdateSpawnChances();
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif