using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : FindTargets
{
    public List<GameObject> targets;
    public GameObject buttons;
    public string activeScene;

    public void Start(){
        targets = FindGoodChar();
    }

    public void removeTarget(GameObject target){
        targets.Remove(target);

        if(targets.Count <= 0){
            buttons.SetActive(true);
            Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLScfXd34AmUOKV92MY-Zv9TzE0Gk-ajhF6PRlfsyR9xaDgSujA/viewform");
             Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdscRqhLkfzKXVuQLDUZZvqDxGOC0qMhoxUVywt9Hdo-8uynw/viewform?usp=sf_link");
        }
    }

    public void RestartGame(){
        Scene scene = SceneManager.GetActiveScene();
        activeScene = scene.name;
        SceneManager.LoadScene(activeScene);
    }

    public void Quit(){
        Application.Quit();
    }
}
