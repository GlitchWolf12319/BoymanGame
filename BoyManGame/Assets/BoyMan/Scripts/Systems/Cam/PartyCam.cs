using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCam : FindTargets
{
    [SerializeField] private GameObject target;
    private Camera cam;
    private bool targetFound;
    private TurnBaseManager tbm;

    void Start()
    {
        tbm = FindObjectOfType<TurnBaseManager>();
    }

    // Find the target GameObject with the "Player" tag
    void FindTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");

        cam = Camera.main;

        if (target != null)
        {
            targetFound = true;
        }
        else
        {
            targetFound = false;
        }
    }

    void FixedUpdate()
    {
        // Check if target exists
        if (target != null)
        {
            // Check if target has been found
            if (targetFound)
            {
                // Check if there is no battle in progress
                if (!tbm.battleInProgress)
                {
                    // Set the camera's position to be offset from the target's position
                    transform.position = new Vector3(target.transform.position.x + 10, cam.transform.position.y, cam.transform.position.z);
                }
            }
        }
        else
        {
            // If target doesn't exist, find a new target
            FindTarget();
        }
    }
}
