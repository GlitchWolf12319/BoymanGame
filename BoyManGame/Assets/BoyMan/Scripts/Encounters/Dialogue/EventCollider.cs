using UnityEngine;

public class EventCollider : MonoBehaviour
{
    public GameObject[] prefabsToInstantiate;
    [SerializeField,HideInInspector]public float[] spawnChances;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null){         
                Movement[] move = FindObjectsOfType<Movement>();
                foreach(Movement Move in move){
                Move.StopEverything();
                Move.enabled = false;
            }
        }

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

    private void UpdateSpawnChances()
    {
        if (spawnChances.Length != prefabsToInstantiate.Length)
        {
            spawnChances = new float[prefabsToInstantiate.Length];
            for (int i = 0; i < spawnChances.Length; i++)
            {
                spawnChances[i] = 1f / spawnChances.Length;
            }
        }

        float totalSpawnChance = 0f;
        for (int i = 0; i < prefabsToInstantiate.Length; i++)
        {
            totalSpawnChance += spawnChances[i];
        }

        for (int i = 0; i < prefabsToInstantiate.Length; i++)
        {
            spawnChances[i] = spawnChances[i] / totalSpawnChance * 100;
        }
    }

    private void OnValidate()
    {
        UpdateSpawnChances();
    }
}
