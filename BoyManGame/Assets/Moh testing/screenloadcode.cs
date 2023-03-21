using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class screenloadcode : MonoBehaviour
{
    [SerializeField] private string levelload;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadscreen()
    {
        SceneManager.LoadScene(levelload);
    }

    public void exitscreen()
    {
        Application.Quit();
    }

}
