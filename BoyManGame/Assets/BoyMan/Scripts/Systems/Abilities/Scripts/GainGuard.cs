using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardAbility", menuName = "CardAbility/Guard")]
public class GainGuard : CardType
{
    //ammount of guard target will gain
    public int guardAmmount;


    //method which returns random guard value
    public int GuardValue(){
        int[] PossibleGuardValues = {5, 10, 13, 7, 11};
        int RandomChoice = Random.Range(0, PossibleGuardValues.Length);
        
        return PossibleGuardValues[RandomChoice];
    }
}
