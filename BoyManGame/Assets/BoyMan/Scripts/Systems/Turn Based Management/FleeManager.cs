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
        TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();
        int counter = 0;
        for(int i = 0; i < tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand.Count; i++){
            if(tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().selected == true){
                counter++;
            }
        }

        if(counter == 0){
            StartCoroutine(Flee());
        }
        
    }

    public IEnumerator Flee(){
        TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();
        
        dice.SetActive(true);
        tbm.turns[tbm.turnCounter].GetComponent<CharTurn>().turnUI.SetActive(false);

        for(int c = 0; c < tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand.Count; c++){
            tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand[c].SetActive(false);
        }



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
            tbm.StartCoroutine(tbm.FleeBattle());
            
        }

        if(roll + 1 < passRoll){
            Debug.Log("Fail");
            yield return new WaitForSeconds(1);
            dice.SetActive(false);
            tbm.turns[tbm.turnCounter].GetComponent<CharTurn>().turnUI.SetActive(true);

            for(int c = 0; c < tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand.Count; c++){
                tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand[c].SetActive(true);
            }

            tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().ClearDeck();
            tbm.turns[tbm.turnCounter].EndTurnButton();
        }
    }
}
