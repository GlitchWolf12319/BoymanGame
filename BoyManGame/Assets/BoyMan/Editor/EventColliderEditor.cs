using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventCollider))]
public class EventColliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EventCollider eventCollider = (EventCollider)target;

        float totalSpawnChance = 0f;
        for (int i = 0; i < eventCollider.prefabsToInstantiate.Length; i++)
        {
            totalSpawnChance += eventCollider.spawnChances[i];
        }

        EditorGUILayout.Space();

        for (int i = 0; i < eventCollider.prefabsToInstantiate.Length; i++)
        {
            float percentageChance = eventCollider.spawnChances[i] / totalSpawnChance * 100;

            EditorGUI.BeginChangeCheck();

            percentageChance = EditorGUILayout.Slider("Spawn Chance for " + eventCollider.prefabsToInstantiate[i].name + " (%):", percentageChance, 0f, 100f);

            if (EditorGUI.EndChangeCheck())
            {
                eventCollider.spawnChances[i] = percentageChance / 100 * totalSpawnChance;
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
