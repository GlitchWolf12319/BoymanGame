using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnBaseManager : FindTargets
{
    [SerializeField]int turnCounter = -1;
    [SerializeField] private List<CharTurn> turns;
    [SerializeField] private List<GameObject> enemiesInBattle;
    [SerializeField] private List<GameObject> heroesInBattle;
    [SerializeField] private List<GameObject> totalCharsInBattle;
    [SerializeField] private GameObject RewardSystem;
    public GameObject disabledUI;
    public bool battleInProgress;

    void Start(){
        disabledUI.transform.localScale = new Vector3(0,0,0);
    }

    public void ChangeTurn(){
        turnCounter++;

        if(turnCounter > turns.Count - 1){
            turnCounter = 0;
        }


        turns[turnCounter].StartCoroutine(turns[turnCounter].StartTurn());
        
    }

    public IEnumerator EnlargeDisabledUI(){
        yield return new WaitForSeconds(1);
        disabledUI.transform.localScale = new Vector3(1,1,1);
    }

    public void SetTurnOrder(){
        enemiesInBattle = FindEnemies();
        heroesInBattle = FindGoodChar();
        totalCharsInBattle.AddRange(FindEnemies());
        totalCharsInBattle.AddRange(FindGoodChar());
        
        for(int i = 0; i < heroesInBattle.Count + enemiesInBattle.Count; i++){
            int randomChoice = Random.Range(0,  totalCharsInBattle.Count);
            turns.Add(totalCharsInBattle[randomChoice].GetComponent<CharTurn>());
            totalCharsInBattle.RemoveAt(randomChoice);
        }

        battleInProgress = true;
    }

    public void RemoveFromTurnOrder(GameObject DeadTarget, string damageType){

        if(!DeadTarget.GetComponent<CharacterController>().dead){

        Debug.Log("Removing " + DeadTarget.name);
        turns.RemoveAt(turns.IndexOf(DeadTarget.GetComponent<CharTurn>()));
        
        if(DeadTarget.GetComponent<CharTurn>().characterType == CharTurn.CharacterType.Player){
            heroesInBattle.Remove(DeadTarget);
        }
        else if(DeadTarget.GetComponent<CharTurn>().characterType == CharTurn.CharacterType.Enemy){
            
            enemiesInBattle.Remove(DeadTarget);
            Debug.Log(damageType);
            if(enemiesInBattle.Count > 0 && damageType != "NormalDamage"){
                ChangeTurn();
            }
        }


        if(isBattleFinished() && heroesInBattle.Count > 0){
            Debug.Log("Battle Finished");
            StartCoroutine(FinishBattle());
            battleInProgress = false;
        }
        }
    }

    public IEnumerator FinishBattle(){
        turns.Clear();
        yield return new WaitForSeconds(3);

        for(int i = 0; i < heroesInBattle.Count; i++){

                for(int c = 0; c < heroesInBattle[i].GetComponent<NewDeckDrawing>().hand.Count; c++){
                    heroesInBattle[i].GetComponent<NewDeckDrawing>().hand[c].GetComponent<Card>().disableHovering = true;
                }

                heroesInBattle[i].GetComponent<CharTurn>().turnUI.SetActive(false);
                heroesInBattle[i].GetComponent<NewDeckDrawing>().ClearDeck();
                heroesInBattle[i].GetComponent<CharacterController>().guard = 0;
                heroesInBattle[i].GetComponent<CharacterController>().igniteStack = 0;
                heroesInBattle[i].GetComponent<CharacterController>().poisonStack = 0;
                heroesInBattle[i].GetComponent<CharTurn>().turnIcon.SetActive(false);
        }
        
        disabledUI.transform.localScale = new Vector3(0,0,0);

        yield return new WaitForSeconds(2.5f);
        GameObject RS = Instantiate(RewardSystem);
        RS.transform.SetAsFirstSibling();
    }  


    public bool isBattleFinished(){
        if(enemiesInBattle.Count <= 0 || heroesInBattle.Count <= 0){
            return true;
        }
        else{
            return false;
        }

    }
}
