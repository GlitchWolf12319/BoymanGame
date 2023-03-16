using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardAbility", menuName = "CardAbility/Heal")]
public class Heal : CardType
{
    public int healAmmount;


    public int GetHealValue(){
        int[] PossibleHealValues = {5, 6, 7, 4, 10};
        int RandomChoice = Random.Range(0, PossibleHealValues.Length);
        
        return PossibleHealValues[RandomChoice];
    }
}
