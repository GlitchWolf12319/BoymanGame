using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySpawnRoom : MonoBehaviour
{
    public GameObject objectToDestroy; // Reference to the object to be destroyed

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the collider belongs to an object with the "Player" tag
            // If true, destroy the object referenced by objectToDestroy
            Destroy(objectToDestroy);
        }
    }
}