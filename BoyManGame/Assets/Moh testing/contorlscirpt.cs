using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class contorlscirpt : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject screens;//this is for the control screen image

    public GameObject mainmenuscreen;//this is for main manu screen image

    public bool controlactive;//this bool is used to check to help tell
    //which button to use to show which screen to switch to

    // Update is called once per frame
    void Update()
    {
       
    }

    public void clickbutton()
    {
       
            screens.SetActive(true);//this set control screen true so it appear
            controlactive= true;
            mainmenuscreen.SetActive(false);//this is for the main menu screen image which is set for false
        //so it wont appear 
       
    }

     public void menubutton()
    {
      
            
            screens.SetActive(false);//this set control screen false so it wont appear
            controlactive= false;
            mainmenuscreen.SetActive(true);//this is for the main menu screen image which is set for true
        //so it appear 
        
    }

}
