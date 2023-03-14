using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class SpawnRoom : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public float[] spawnChances;
    public GameObject spawnPosition;
   public TextMeshProUGUI  counterText;
private static int numColliders;
  private bool hasSpawned;

    
    private void Start()
    {
        // Find the counterText by looking for the tag "decentCounter"
        counterText = GameObject.FindWithTag("decentCounter").GetComponent<TextMeshProUGUI>();
    }

  private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player") && !hasSpawned)
    {
        numColliders++;
        counterText.text = GetCountString(numColliders) + " Decent";

        // Flash the text
        counterText.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo);

        // Calculate the total spawn chance
        float totalSpawnChance = 0f;
        for (int i = 0; i < objectsToSpawn.Length; i++)
        {
            totalSpawnChance += spawnChances[i];
        }

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
            suffix = "th";
        }

        return countString + suffix;
    }
}
