using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    [SerializeField]int turnCounter = -1;
    [SerializeField] private List<CharTurn> turns;

    void Update(){
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
        turns[turnCounter].tbm = this;
    }

    public void RemoveFromTurnOrder(GameObject DeadTarget){
        turns.RemoveAt(turns.IndexOf(DeadTarget.GetComponent<CharTurn>()));
    }  
}
