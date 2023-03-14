using UnityEngine;

public class EventCollider : MonoBehaviour
{
    public GameObject[] prefabsToInstantiate;
    public float[] spawnChances;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float totalSpawnChance = 0f;
            for (int i = 0; i < prefabsToInstantiate.Length; i++)
            {
                totalSpawnChance += spawnChances[i];
            }

            float randomValue = Random.value * totalSpawnChance;
            float cumulativePercentage = 0f;
            int objectIndex = -1;

            for (int i = 0; i < prefabsToInstantiate.Length; i++)
            {
                cumulativePercentage += spawnChances[i];
                if (randomValue <= cumulativePercentage)
                {
                    objectIndex = i;
                    break;
                }
            }

            if (objectIndex >= 0 && objectIndex < prefabsToInstantiate.Length)
            {
                GameObject canvasGameObject = GameObject.FindGameObjectWithTag("canvas");
               
                if (canvasGameObject != null)
                {
                    Canvas canvas = canvasGameObject.GetComponent<Canvas>();
                    if (canvas != null)
                    {
                        
                        GameObject instantiatedPrefab = Instantiate(prefabsToInstantiate[objectIndex], canvas.transform);
                      
                    }
                }
            }

            // Destroy the GameObject that has the EventCollider component attached
            Destroy(gameObject);
        }
    }
}