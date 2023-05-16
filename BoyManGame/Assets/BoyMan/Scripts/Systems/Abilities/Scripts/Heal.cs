using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardAbility", menuName = "CardAbility/Heal")]
public class Heal : CardType
{
    //ammount target will be healed by
    public int healAmmount;


    //random method which returns random heal value
    public int GetHealValue(){
        int[] PossibleHealValues = {5, 6, 7, 4, 10};
        int RandomChoice = Random.Range(0, PossibleHealValues.Length);
        
        return PossibleHealValues[RandomChoice];
    }
}
