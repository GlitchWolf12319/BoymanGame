using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TurnBanner : MonoBehaviour
{
    public enum Turn { Player, Enemy };
    public Turn currentTurn = Turn.Player;
       [SerializeField] private TextMeshProUGUI turnText;

    void Start()
    {
        turnText.text = "Player's Turn";
        EndTurn();
    }

    public void EndTurn()
    {
        if (currentTurn == Turn.Player)
        {
            currentTurn = Turn.Enemy;
            turnText.text = "Enemy's Turn";
        }
        else if (currentTurn == Turn.Enemy)
        {
            currentTurn = Turn.Player;
            turnText.text = "Player's Turn";
        }
    }
}