using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPartyStatus : MonoBehaviour
{
    private GameObject boyman;
    private GameObject jane;
    private GameObject oslo;
    private GameObject casper;

    public GameObject boymanJane;
    public GameObject boymanOslo;
    public GameObject boymanCasper;
    public GameObject janeOslo;
    public GameObject janeCasper;
    public GameObject osloCasper;

    private bool foundCharactersToTalk;
	private bool hit;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hit) 
        {
			hit = true;
            boyman = GameObject.Find("BoyMan");
            jane = GameObject.Find("Jane");
            oslo = GameObject.Find("Oslo");
            casper = GameObject.Find("Casper");
			 GameObject canvasGameObject = GameObject.FindGameObjectWithTag("canvas");          
List<GameObject> characters = new List<GameObject>();

if (boyman != null)
{
    characters.Add(boyman);
}
if (jane != null)
{
    characters.Add(jane);
}
if (oslo != null)
{
    characters.Add(oslo);
}
if (casper != null)
{
    characters.Add(casper);
}


            foundCharactersToTalk = false;
while (!foundCharactersToTalk && characters.Count >= 2)
{

    Debug.Log(characters.Count);
    int index1 = Random.Range(0, characters.Count);
    int index2 = Random.Range(0, characters.Count);


    // Generate new indices until they are different and within the valid range
    while (index2 == index1)
    {
        index2 = Random.Range(0, characters.Count);
     
    }
 
    GameObject character1 = characters[index1];
    GameObject character2 = characters[index2];

  
    if (character1 != null && character2 != null)
    {
        Debug.Log(character1.name + " talks to " + character2.name);

        if (character1.name == "BoyMan")
        {
            if (character2.name == "Jane")
            {		  
                if (canvasGameObject != null)
                {
                    Canvas canvas = canvasGameObject.GetComponent<Canvas>();
                    if (canvas != null)
                    {

                        GameObject instantiatedPrefab =  Instantiate(boymanJane, transform.position, Quaternion.identity);

                    }
                }               
            }
            else if (character2.name == "Oslo")
            {
				  if (canvasGameObject != null)
                {
                    Canvas canvas = canvasGameObject.GetComponent<Canvas>();
                    if (canvas != null)
                    {

                        GameObject instantiatedPrefab =  Instantiate(boymanOslo, transform.position, Quaternion.identity);
                    }
                }   
         
            }
            else if (character2.name == "Casper")
            {

				  if (canvasGameObject != null)
                {
                    Canvas canvas = canvasGameObject.GetComponent<Canvas>();
                    if (canvas != null)
                    {

                        GameObject instantiatedPrefab =    Instantiate(boymanCasper, transform.position, Quaternion.identity);
                    }
                }   
              
            }
        }
        else if (character1.name == "Jane")
        {
            if (character2.name == "Oslo")
            {
				  if (canvasGameObject != null)
                {
                    Canvas canvas = canvasGameObject.GetComponent<Canvas>();
                    if (canvas != null)
                    {

                        GameObject instantiatedPrefab =    Instantiate(janeOslo, transform.position, Quaternion.identity);
                    }
                }   
               
            }
            else if (character2.name == "Casper")
            {
				  if (canvasGameObject != null)
                {
                    Canvas canvas = canvasGameObject.GetComponent<Canvas>();
                    if (canvas != null)
                    {

                        GameObject instantiatedPrefab =    Instantiate(janeCasper, transform.position, Quaternion.identity);
                    }
                }  
               
            }
            else if (character2.name == "BoyMan")
            {
				  if (canvasGameObject != null)
                {
                    Canvas canvas = canvasGameObject.GetComponent<Canvas>();
                    if (canvas != null)
                    {

                        GameObject instantiatedPrefab =    Instantiate(boymanJane, transform.position, Quaternion.identity);
                    }
                }  
               
            }
        }
        else if (character1.name == "Oslo" && character2.name == "Casper")
        {
			  if (canvasGameObject != null)
                {
                    Canvas canvas = canvasGameObject.GetComponent<Canvas>();
                    if (canvas != null)
                    {

                        GameObject instantiatedPrefab =   Instantiate(osloCasper, transform.position, Quaternion.identity);
                    }
                }  
            
        }

        foundCharactersToTalk = true;
      

        // Sort indices in descending order before removing from list
        int largerIndex = Mathf.Max(index1, index2);
        int smallerIndex = Mathf.Min(index1, index2);

        // Check that indices are within range before removing from list
        if (largerIndex < characters.Count && smallerIndex >= 0 && smallerIndex < largerIndex)
        {
            characters.RemoveAt(largerIndex);
            characters.RemoveAt(smallerIndex);
        }
    }
   }
}
		}
	}

