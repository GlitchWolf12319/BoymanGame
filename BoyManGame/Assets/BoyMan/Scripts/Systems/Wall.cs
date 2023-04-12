using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private Vector3 startingPosition;
    private bool isMovingBack;

    // The amount of time it takes to move the player back to their starting position
    public float moveBackTime = 0.5f;

    // The distance the player will be moved back
    
    public float moveBackDistance = 1.0f;

    void Start()
    {
        Debug.Log(startingPosition);
        // Store the starting position of the player
        startingPosition = transform.position;
    }

    void Update()
    {
        if (isMovingBack)
        {
            // Calculate the new position for the player
            Vector3 newPosition = Vector3.MoveTowards(transform.position, startingPosition, moveBackDistance * Time.deltaTime / moveBackTime);
            
            // Move the player to the new position
            transform.position = newPosition;

            // Stop moving the player back once they reach their starting position
            if (transform.position == startingPosition)
            {
                isMovingBack = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with a trigger tagged as "Obstacle"
        if (other.CompareTag("Obstacle"))
        {
            // Start moving the player back to their starting position
            isMovingBack = true;
        }
    }
}