using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            objectToActivate.SetActive(true);
             StartCoroutine(WaitForSeconds());    
        }
    }       
   

      private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(0.3f);

              GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null){         
                Wall[] wall = FindObjectsOfType<Wall>();
                foreach(Wall Wall in wall){
                Wall.SetStartingPostion();
    }
            }
            yield return new WaitForSeconds(0.1f);
            Destroy(this.gameObject);
    }
}
