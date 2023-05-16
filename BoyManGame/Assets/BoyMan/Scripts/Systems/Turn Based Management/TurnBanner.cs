using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TurnBanner : MonoBehaviour
{
    public enum Turn { Player, Enemy }; // Enum to represent the turn types (Player or Enemy)
    public Turn currentTurn = Turn.Player; // Current turn variable, initialized as Player's turn
    public TextMeshProUGUI turnText; // Reference to the text component displaying the turn information
    public TextMeshProUGUI turnCounterText; // Reference to the text component displaying the turn counter

    void Start()
    {
        turnText.text = "Player's Turn"; // Set the initial turn text to "Player's Turn"
    }

    public void EndTurn()
    {
        if (currentTurn == Turn.Player)
        {
            currentTurn = Turn.Enemy; // Switch the turn to Enemy's turn
            turnText.text = "Enemy's Turn"; // Update the turn text to "Enemy's Turn"
        }
        else if (currentTurn == Turn.Enemy)
        {
            currentTurn = Turn.Player; // Switch the turn to Player's turn
            turnText.text = "Player's Turn"; // Update the turn text to "Player's Turn"
        }
    }
}