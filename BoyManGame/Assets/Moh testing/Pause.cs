using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Pause : MonoBehaviour
{
    public bool pausegame = false;
    // Start is called before the first frame update

   
    public GameObject textpause;

    // Update is called once per frame
    void Update()
    {
       

       if(Input.GetKeyDown(KeyCode.Escape))
       {
       
            if(pausegame==false)
            {
                Cursor.lockState = CursorLockMode.Locked;//mouse cursosr 21st marh
                Cursor.visible = false;//21st marh

                pausegame = true;
                textpause.SetActive(true);
                Time.timeScale =0;
            }
            else{
                 Cursor.lockState = CursorLockMode.None;//mouse cursosr 21st marh
                Cursor.visible = true;//21st marh
                
                pausegame = false;
                textpause.SetActive(false);
                Time.timeScale =1;
            }

        
       }
    }

 



   
}
