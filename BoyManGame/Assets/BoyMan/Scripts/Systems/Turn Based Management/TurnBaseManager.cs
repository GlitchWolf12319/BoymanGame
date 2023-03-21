using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseManager : FindTargets
{
    [SerializeField]int turnCounter = -1;
    [SerializeField] private List<CharTurn> turns;
    [SerializeField] private List<GameObject> enemiesInBattle;
    [SerializeField] private List<GameObject> heroesInBattle;
    [SerializeField] private List<GameObject> totalCharsInBattle;
    [SerializeField] private GameObject RewardSystem;

    void Update(){

        if(Input.GetKeyDown(KeyCode.Return)){
            SetTurnOrder();
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            ChangeTurn();
        }
    }

    public void ChangeTurn(){
        turnCounter++;

        if(turnCounter > turns.Count - 1){
            turnCounter = 0;
        }

        turns[turnCounter].StartCoroutine(turns[turnCounter].StartTurn());
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
    }

    public void RemoveFromTurnOrder(GameObject DeadTarget){
        turns.RemoveAt(turns.IndexOf(DeadTarget.GetComponent<CharTurn>()));
        if(DeadTarget.GetComponent<CharTurn>().characterType == CharTurn.CharacterType.Player){
            heroesInBattle.Remove(DeadTarget);
        }
        else if(DeadTarget.GetComponent<CharTurn>().characterType == CharTurn.CharacterType.Enemy){
            enemiesInBattle.Remove(DeadTarget);
        }


        if(isBattleFinished()){
            StartCoroutine(FinishBattle());
        }
    }

    public IEnumerator FinishBattle(){
        turns.Clear();
        yield return new WaitForSeconds(3);
        Instantiate(RewardSystem);
        yield return new WaitForSeconds(3);
        for(int i = 0; i < heroesInBattle.Count; i++){
                heroesInBattle[i].GetComponent<DeckDrawing>().MoveDeckToDiscardPile();
                
                foreach(var card in heroesInBattle[i].GetComponent<DeckDrawing>().discardPile){
                    heroesInBattle[i].GetComponent<DeckDrawing>().deck.Add(card);
                }
            }

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
