using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPartyStatus : MonoBehaviour
{
    public bool Boyman, Jane, Oslo, Casper;

    private void OnTriggerEnter(Collider other){
	if(other.name == "Trigger"){
		CheckParty();
	}
    }

    void CheckParty(){
	    Boyman = GameObject.Find("Boyman");
	    Jane = GameObject.Find("jane");
	    Oslo = GameObject.Find("oslo");
	    Casper = GameObject.Find("casper");
}
}
