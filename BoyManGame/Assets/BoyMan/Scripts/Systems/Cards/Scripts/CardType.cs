using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardType : ScriptableObject
{
    //declare card type variables
    public DealDamage dealDamage;
    public PoisonEffect poisonEffect;
    public IgniteEffect igniteEffect;
    public Pull pull;
    public Push push;
    public Retreat retreat;
    public GainGuard guard;
    public Heal heal;
    public Chilled chilled;
    public Invisible invisible;
    public DealPartyDamage dealPartyDamage;
    public HealParty healParty;
    public IgniteParty igniteParty;
    public Weaken weaken;
    
}
