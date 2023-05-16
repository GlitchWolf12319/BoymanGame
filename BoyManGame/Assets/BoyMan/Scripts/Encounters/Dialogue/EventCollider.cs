using UnityEngine;

public class EventCollider : MonoBehaviour
{
    public GameObject[] prefabsToInstantiate;  // Array of prefabs to be instantiated
    [SerializeField, HideInInspector] public float[] spawnChances;  // Array of spawn chances for each prefab

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.GetComponent<BoxCollider>().enabled = false;  // Disable the box collider on collision
            Debug.Log("Collision " + other.gameObject.name);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Movement[] move = FindObjectsOfType<Movement>();
                foreach (Movement Move in move)
                {
                    Move.StopEverything();  // Stop player movement
                    Move.enabled = false;
                }
            }

            float totalSpawnChance = 0f;
            for (int i = 0; i < prefabsToInstantiate.Length; i++)
            {
                totalSpawnChance += spawnChances[i];  // Calculate the total spawn chance
            }

            float randomValue = Random.value * totalSpawnChance;  // Generate a random value based on total spawn chance
            float cumulativePercentage = 0f;
            int objectIndex = -1;

            for (int i = 0; i < prefabsToInstantiate.Length; i++)
            {
                cumulativePercentage += spawnChances[i];  // Calculate cumulative percentage
                if (randomValue <= cumulativePercentage)
                {
                    objectIndex = i;  // Determine the index of the chosen prefab to be instantiated
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
                        // Instantiate the chosen prefab as a child of the canvas
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
                spawnChances[i] = 1f / spawnChances.Length;  // Set equal spawn chances for each prefab
            }
        }

        float totalSpawnChance = 0f;
        for (int i = 0; i < prefabsToInstantiate.Length; i++)
        {
            totalSpawnChance += spawnChances[i];  // Calculate the total spawn chance
        }

        for (int i = 0; i < prefabsToInstantiate.Length; i++)
        {
            spawnChances[i] = spawnChances[i] / totalSpawnChance * 100;  // Convert spawn chances to percentages
        }
    }

    private void OnValidate()
    {
        UpdateSpawnChances();  // Update spawn chances when the script is validated in the editor
    }
}
