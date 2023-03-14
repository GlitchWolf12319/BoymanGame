using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{   
    //The transform component of the player object 
    public Transform player;
    //speed of the camera lag
    public float lagSpeed = 2.0f;
    //offset of the camera
    private Vector3 offset;
    //velocity of the camera movement
    private Vector3 velocity;



    //method that runs after all other updates have been processed
    void LateUpdate()
    {
        if(player != null){
        //calculates the target position of the camera based on the player's position and offset
        Vector3 targetPos = player.position + offset;
        //smoothly moves the camera to the target position with lag
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, lagSpeed);
        }
    }
}