using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardAbility", menuName = "CardAbility/Poison")]
public class PoisonEffect : CardType
{
    //ammount of poison stacks target will get
    public int poisonStack;
    public int poisonAmmount;
}
