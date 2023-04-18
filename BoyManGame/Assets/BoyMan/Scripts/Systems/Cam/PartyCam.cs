using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCam : FindTargets
{
    [SerializeField]private GameObject target;
    private Camera cam;
    private bool targetFound;
    private TurnBaseManager tbm;


    void Start(){
        tbm = FindObjectOfType<TurnBaseManager>();
    }

    void FindTarget(){
        target = GameObject.FindGameObjectWithTag("Player");


        cam = Camera.main;

        if(target != null){
            targetFound = true;
        }
        else{
            targetFound = false;
        }
    }


    void FixedUpdate(){
        if(target != null){
            if(targetFound){
                if(!tbm.battleInProgress){
                    transform.position = new Vector3(target.transform.position.x + 10, cam.transform.position.y, cam.transform.position.z);
                }
            }
        }
        else{
            FindTarget();
        }   
    }
}
