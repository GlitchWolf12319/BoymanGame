using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : FindTargets
{

    [Header("Place enemies to spawn in battle here")]
    public GameObject[] enemyPrefab;
    public Vector3[] ScaleSize;

    // The list of enemies spawned by this spawner
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    // The position of the spawner
    private Vector3 spawnerPosition;

    [Header("Spawn Distance away from player")]
    public float spawnDistance;
    [Header("Spawn Distance away from each other")]
    public float enemySpawnDistance;

    public GameObject player;
    public TurnBaseManager tbm;

    void Start(){
        spawnerPosition = transform.position;
        tbm = FindObjectOfType<TurnBaseManager>();
        StartCoroutine(BattleStarter());
    }


    public IEnumerator BattleStarter(){
        yield return new WaitForSeconds(3);
        player = GameObject.Find("BoyMan");
        if(player == null){
            player = GameObject.Find("Jane");
        }
        SpawnEnemies();
        tbm.SetTurnOrder();
        tbm.ChangeTurn();
    }

    // public void OnTriggerEnter(Collider other){
    //     if(other.tag == "Player"){
    //         player = other.gameObject;
    //         BattleStarter();
    //         this.GetComponent<BoxCollider>().enabled = false;
    //     }
    // }

    private void SpawnEnemies()
    {
        // Get the position of the player
        Vector3 playerPosition = player.transform.position;

        // Calculate the position to spawn the enemies
        Vector3 spawnPosition = playerPosition + player.transform.right * spawnDistance;
        Vector3 FlySpawnPosition = spawnPosition + player.transform.up * 1.5f;

        // Spawn the enemies
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            Vector3 enemySpawnPosition = new Vector3();

            if(enemyPrefab[i].name.Contains("Bat")){
                // Calculate the position for this enemy spawn
                enemySpawnPosition = FlySpawnPosition + player.transform.right * i * enemySpawnDistance;
            }
            else{
                enemySpawnPosition = spawnPosition + player.transform.right * i * enemySpawnDistance;
            }
            

            // Instantiate the enemy prefab at the spawn position
            GameObject enemy = Instantiate(enemyPrefab[i], enemySpawnPosition, Quaternion.identity);
            enemy.transform.DOScale(ScaleSize[i], 1f);

            // Add the enemy to the spawnedEnemies list
            spawnedEnemies.Add(enemy);
        
        }

        
    }
}
