using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardAbility", menuName = "CardAbility/Ignite")]
public class IgniteEffect : CardType
{
    //ammount of ignite stacks target will get
    public int IgniteStack;
    public int IgniteAmmount;
}
