using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Enemy", menuName = "EnemyTurn")]
public class EnemyTurn : ScriptableObject
{
    public int thinkTime;
    public enum Target {Enemy, Player, Party}
    public Target target;
    public PerTurnPattern[] TurnMoves;


    public void SetTarget(int index){
        Debug.Log(index);
        //set appropriate target depending on the attack ability
        for(int i = 0; i < TurnMoves[index].cards.Length; i++){
            if(TurnMoves[index].cards[i].push != null || TurnMoves[index].cards[i].retreat != null || TurnMoves[index].cards[i].guard != null || TurnMoves[index].cards[i].heal != null){
                target = Target.Enemy;
            }

            if(TurnMoves[index].cards[i].poisonEffect != null || TurnMoves[index].cards[i].igniteEffect != null || TurnMoves[index].cards[i].dealDamage != null || TurnMoves[index].cards[i].pull != null){
                target = Target.Player;
            }
        }
    }
}

[Serializable]
public class PerTurnPattern{
    public CardType[] cards;
}
