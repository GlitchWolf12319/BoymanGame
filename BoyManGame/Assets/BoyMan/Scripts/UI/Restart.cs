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
