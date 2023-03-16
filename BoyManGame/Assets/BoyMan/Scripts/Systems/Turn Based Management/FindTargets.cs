using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargets : MonoBehaviour
{
    public List<GameObject> FindEnemies(){
        List<GameObject> enemiesInScene = new List<GameObject>();

        foreach(GameObject enemies in GameObject.FindGameObjectsWithTag("Enemy")){
            enemiesInScene.Add(enemies);
        }

        return enemiesInScene;
    }

    public List<GameObject> FindGoodChar(){
        List<GameObject> goodCharsinScene = new List<GameObject>();

        foreach(GameObject players in GameObject.FindGameObjectsWithTag("Player")){
            goodCharsinScene.Add(players);
        }

        if(goodCharsinScene.Count > 0){
            return goodCharsinScene;
        }
        else{
            return null;
        }

        
    }
}
