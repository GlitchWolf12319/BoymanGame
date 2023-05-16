using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    // Called when another collider enters this collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider belongs to the player object
        if (other.gameObject.CompareTag("Player"))
        {
            // Activate the specified object
            objectToActivate.SetActive(true);

            // Start a coroutine to delay the execution of the remaining code
            StartCoroutine(WaitForSeconds());
        }
    }

    // Coroutine to introduce a delay
    private IEnumerator WaitForSeconds()
    {
        // Wait for 0.3 seconds
        yield return new WaitForSeconds(0.3f);

        // Find the player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // If the player object is found
        if (player != null)
        {
            // Find all Wall objects in the scene
            Wall[] walls = FindObjectsOfType<Wall>();

            // Set the starting position for each Wall
            foreach (Wall wall in walls)
            {
                wall.SetStartingPostion();
            }
        }

        // Wait for an additional 0.1 seconds
        yield return new WaitForSeconds(0.1f);

        // Destroy this game object
        Destroy(this.gameObject);
    }
}