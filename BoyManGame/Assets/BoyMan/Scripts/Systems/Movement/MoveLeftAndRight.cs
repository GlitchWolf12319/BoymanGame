using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftAndRight : MonoBehaviour
{
    public float speed = 5f; // The movement speed

    void Update()
    {
        // Get the horizontal input axis
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate the new position
        Vector3 newPosition = transform.position + new Vector3(horizontalInput * speed * Time.deltaTime, 0f, 0f);

        // Move the object to the new position
        transform.position = newPosition;
    }
}
