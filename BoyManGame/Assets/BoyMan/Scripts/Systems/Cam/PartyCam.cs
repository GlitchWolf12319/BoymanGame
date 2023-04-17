using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCam : MonoBehaviour
{
    [SerializeField]private GameObject target;
    private Camera cam;
    private bool targetFound;

    void FindTarget(){
        target = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;

        if(target != null){
            targetFound = true;
        }
        else{
            targetFound = false;
            FindTarget();
        }
    }


    void FixedUpdate(){
        if(target != null){
            if(targetFound){
                transform.position = new Vector3(target.transform.position.x + 10, cam.transform.position.y, cam.transform.position.z);
            }
        }
        else{
            FindTarget();
        }
        
    }
}
