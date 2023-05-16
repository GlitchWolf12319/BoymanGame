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


    void Awake()
    {
        // Find the dice object in the scene with the "Dice" tag and deactivate it
        dice = GameObject.FindGameObjectWithTag("Dice");
        dice.SetActive(false);
    }

    public void FleeButton()
    {
        TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();
        int counter = 0;

        // Count the number of selected cards in the current player's hand
        for (int i = 0; i < tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand.Count; i++)
        {
            if (tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().selected == true)
            {
                counter++;
            }
        }

        if (counter == 0)
        {
            StartCoroutine(Flee());
        }
    }

    public IEnumerator Flee()
    {
        TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();

        // Activate the dice object and hide the turn UI
        dice.SetActive(true);
        tbm.turns[tbm.turnCounter].GetComponent<CharTurn>().turnUI.SetActive(false);

        // Deactivate all cards in the current player's hand
        for (int c = 0; c < tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand.Count; c++)
        {
            tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand[c].SetActive(false);
        }

        // Roll the dice animation
        for (int i = 0; i < 20; i++)
        {
            int noRoll = Random.Range(1, 20);
            spriteRend.sprite = diceFaces[noRoll];
            yield return new WaitForSeconds(0.1f);
        }

        // Roll the final dice result
        int roll = Random.Range(1, 20);
        spriteRend.sprite = diceFaces[roll];

        // Check if the roll meets the required passing roll value
        if (roll + 1 >= passRoll)
        {
            Debug.Log("Pass");
            yield return new WaitForSeconds(1);
            dice.SetActive(false);
            tbm.StartCoroutine(tbm.FleeBattle());
        }
        else
        {
            Debug.Log("Fail");
            yield return new WaitForSeconds(1);
            dice.SetActive(false);
            tbm.turns[tbm.turnCounter].GetComponent<CharTurn>().turnUI.SetActive(true);

            // Reactivate all cards in the current player's hand
            for (int c = 0; c < tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand.Count; c++)
            {
                tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().hand[c].SetActive(true);
            }

            // Clear the player's deck and end their turn
            tbm.turns[tbm.turnCounter].GetComponent<NewDeckDrawing>().ClearDeck();
            tbm.turns[tbm.turnCounter].EndTurnButton();
        }
    }
}