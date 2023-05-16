using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardAbility", menuName = "CardAbility/Damage")]
public class DealDamage : CardType
{
    //ammount of damage target will take
    public int damageAmmount;


    //method which returns random damage value
    public int GetDamageValue(){
        int[] PossibleDamageValues = {5, 10, 13, 7, 11};
        int RandomChoice = Random.Range(0, PossibleDamageValues.Length);
        
        return PossibleDamageValues[RandomChoice];
    }
}
