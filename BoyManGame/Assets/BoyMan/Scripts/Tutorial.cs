using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{

    public Image imageComponent;
    public Sprite[] imageArray;
    public bool TutorialStartBool;


    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        imageComponent.enabled = false;





    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
     
        // Wait for 5 seconds
        TutorialStart();
    }


    private void TutorialStart()
    {
 
        Debug.Log("SpawnTutorialImage");
        StartCoroutine(WaitForTwoSeconds());
    }

   private IEnumerator WaitForTwoSeconds()
    {
        yield return new WaitForSeconds(5f);

            TutorialStartBool = true;
    }
    private void Update()
  {
    // Check for mouse click
    if (Input.GetMouseButtonDown(0) && TutorialStartBool == true)
    {
        // Increment the index and wrap around if necessary
        currentIndex = (currentIndex + 1) % imageArray.Length;

        // Show the next image in the array
        imageComponent.sprite = imageArray[currentIndex];
    }

    if (TutorialStartBool)
    {
        TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();
        if (tbm.APlayersTurn == true)
        {
            Debug.Log("Tutorial Player Turn");
            imageComponent.enabled = true;
            // Show the first image in the array
            imageComponent.sprite = imageArray[currentIndex];
            Time.timeScale = 0;

            // Check if the current index is at the end of the array
            if (currentIndex == imageArray.Length - 1)
            {
                // Reset time scale and disable the image component
                Time.timeScale = 1;
                imageComponent.enabled = false;

                // Destroy the tutorial game object
                Destroy(gameObject);
            }
        }
    }
}
}

