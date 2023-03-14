using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySpawnRoom : MonoBehaviour
{
 public GameObject objectToDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(objectToDestroy);
        }
    }

    
}