using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleeManager : MonoBehaviour
{
    public GameObject dice;
    public int passRoll;

    public Image spriteRend;
    public Sprite[] diceFaces;


    void Awake(){
        dice = GameObject.FindGameObjectWithTag("Dice");
        dice.SetActive(false);
    }

    public void FleeButton(){
        StartCoroutine(Flee());
    }

    public IEnumerator Flee(){

        dice.SetActive(true);

        for(int i = 0; i < 20; i++){
            int noRoll = Random.Range(1, 20);
            spriteRend.sprite = diceFaces[noRoll];
            yield return new WaitForSeconds(0.1f);
        }

        int roll = Random.Range(1, 20);
        spriteRend.sprite = diceFaces[roll];

        if(roll + 1 >= passRoll){
            Debug.Log("Pass");
            yield return new WaitForSeconds(1);
            dice.SetActive(false);
            TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();
            tbm.StartCoroutine(tbm.FleeBattle());
        }

        if(roll + 1 < passRoll){
            Debug.Log("Fail");
            yield return new WaitForSeconds(1);
            dice.SetActive(false);
            TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();
            tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().ClearDeck();
            tbm.turns[tbm.turnCounter].EndTurnButton();
        }
    }
}
