using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class SpawnRoom : MonoBehaviour
{
  
    [SerializeField, HideInInspector]public float[] spawnChances;
    public GameObject spawnPosition;
    public TextMeshProUGUI counterText;
    private static int numColliders;
    private bool hasSpawned;
      public GameObject[] objectsToSpawn;

    private void Start()
    {
        // Find the counterText by looking for the tag "decentCounter"
        counterText = GameObject.FindWithTag("decentCounter").GetComponent<TextMeshProUGUI>();
        // Resize the spawnChances array to match the length of objectsToSpawn
        System.Array.Resize(ref spawnChances, objectsToSpawn.Length);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            numColliders++;
            counterText.text = GetCountString(numColliders) + " Descent ";

            // Flash the text
            counterText.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo);

            // Calculate the total spawn chance
            float totalSpawnChance = GetTotalSpawnChance();

            // Randomly select an object to spawn based on their spawn chances
            float randomValue = Random.value * totalSpawnChance;
            float cumulativePercentage = 0f;
            int objectIndex = -1;

            for (int i = 0; i < objectsToSpawn.Length; i++)
            {
                cumulativePercentage += spawnChances[i];
                if (randomValue <= cumulativePercentage)
                {
                    objectIndex = i;
                    break;
                }
            }

            if (objectIndex >= 0)
            {
                Instantiate(objectsToSpawn[objectIndex], spawnPosition.transform.position, Quaternion.identity);
                hasSpawned = true;
            }
        }
    }

    private string GetCountString(int count)
    {
        string countString = count.ToString();
        string suffix = "";

        // Check the last digit of the count to determine the suffix
        switch (count % 10)
        {
            case 1:
                suffix = "st";
                break;
            case 2:
                suffix = "nd";
                break;
            case 3:
                suffix = "rd";
                break;
            default:
                suffix = "th";
                break;
        }

        // Special case for "11th", "12th", "13th"
        if (count % 100 >= 11 && count % 100 <= 13)
        {
            suffix= "th";
}
   return countString + suffix;
}
public void SetSpawnChanceTo100(int index)
{
    if (index >= 0 && index < spawnChances.Length)
    {
        for (int i = 0; i < spawnChances.Length; i++)
        {
            if (i == index)
            {
                spawnChances[i] = 100f;
            }
            else
            {
                spawnChances[i] = 0f;
            }
        }

        UpdateSpawnChances();
    }
}
public void UpdateSpawnChances()
{
    float totalSpawnChance = 0f;
    for (int i = 0; i < spawnChances.Length; i++)
    {
        if (spawnChances[i] < 0f)
        {
            spawnChances[i] = 0f;
        }
        totalSpawnChance += spawnChances[i];
    }

    if (totalSpawnChance > 0f)
    {
        float scaleFactor = 100f / totalSpawnChance;
        for (int i = 0; i < spawnChances.Length; i++)
        {
            spawnChances[i] *= scaleFactor;
        }
    }
}
private float GetTotalSpawnChance()
{
    float totalSpawnChance = 0f;
    for (int i = 0; i < objectsToSpawn.Length; i++)
    {
        totalSpawnChance += spawnChances[i];
    }
    return totalSpawnChance;
}

private void OnValidate()
{
    // Auto-populate spawnChances array to match the length of objectsToSpawn array
    if (objectsToSpawn != null && spawnChances != null && objectsToSpawn.Length != spawnChances.Length)
    {
        spawnChances = new float[objectsToSpawn.Length];
        for (int i = 0; i < spawnChances.Length; i++)
        {
            spawnChances[i] = 1f / spawnChances.Length; // Set all values to be equal
        }
    }
}
public void SetSpawnChance(int index, float value)
{
    // Calculate the new spawn chance for the object
    float totalSpawnChance = GetTotalSpawnChance();
    float newSpawnChance = value / 100f * totalSpawnChance;

    // Update the spawn chance for the object
    spawnChances[index] = newSpawnChance;

    
}
}
            