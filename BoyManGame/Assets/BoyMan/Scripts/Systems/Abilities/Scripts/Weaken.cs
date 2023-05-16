using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardAbility", menuName = "CardAbility/Weaken")]
public class Weaken : CardType
{
    //ammount of weaken stacks target will get
    public int WeakenStack;
}
