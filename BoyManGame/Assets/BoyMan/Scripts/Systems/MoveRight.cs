using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveRight : MonoBehaviour
{
    public Transform moveTarget; // Transform to move to
    public float moveDuration = 1f; // Duration of the movement animation

    private bool canMove = true; // Flag to prevent multiple movements at the same time

    private void Update()
    {
        // Listen for keyboard input
        if (canMove && Input.GetKeyDown(KeyCode.D))
        {
            Move(); // Move the player to the target
        }
    }

    
    public void Move()
    {
        
        // Calculate the distance to move
        float distance = moveTarget.position.x - transform.position.x;

        // Move the player to the target using DOTween
        transform.DOMoveX(moveTarget.position.x, moveDuration).OnComplete(() =>
        {
            canMove = true; // Set the flag to allow movement again
        });
        canMove = false; // Set the flag to prevent multiple movements at the same time
    }
}