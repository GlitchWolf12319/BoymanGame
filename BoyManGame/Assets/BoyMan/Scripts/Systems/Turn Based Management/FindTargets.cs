using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargets : MonoBehaviour
{
    public List<GameObject> FindEnemies()
    {
        List<GameObject> enemiesInScene = new List<GameObject>();

        // Find all game objects with the "Enemy" tag and add them to the list
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemiesInScene.Add(enemy);
        }

        return enemiesInScene;
    }

    public List<GameObject> FindGoodChar()
    {
        List<GameObject> goodCharsInScene = new List<GameObject>();

        // Find all game objects with the "Player" tag and add them to the list if they have an invisible stack of 0 or less
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<CharacterController>().invisibleStack <= 0)
            {
                goodCharsInScene.Add(player);
            }
        }

        if (goodCharsInScene.Count > 0)
        {
            return goodCharsInScene; // Return the list of good characters if there are any
        }
        else
        {
            return null; // Return null if no good characters are found
        }
    }
}