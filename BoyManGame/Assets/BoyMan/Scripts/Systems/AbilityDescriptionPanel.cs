using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityDescriptionPanel : MonoBehaviour
{
    public CardTemplate card;
    public TMP_Text Title;
    public TMP_Text Description;


    void Start(){

        card = transform.parent.GetComponent<Card>().card;

        if(card.ability[0].dealDamage != null){
            Title.color = Color.red;
            Title.text = "Damage";
            Description.text = "Deals X damage ammount to target";
        }

        if(card.ability[0].igniteEffect != null){
            Title.color = Color.red;
            Title.text = "Ignite";
            Description.text = "At the beginning of a Ignite enemy's turn, they take damage equal to the # of ignite stacks they have. Then the ignite stack decreases by 1";
        }

        if(card.ability[0].weaken != null){
            Title.color = Color.magenta;
            Title.text = "Weaken";
            Description.text = "Enemies deal 25% less damage with attacks";
        }

        if(card.ability[0].poisonEffect != null){
            Title.color = Color.green;
            Title.text = "Poison";
            Description.text = "At the beginning of targets turn, the target loses X HP and 1 stack of poison.";
        }

        if(card.ability[0].guard != null){
            Title.color = Color.gray;
            Title.text = "Guard";
            Description.text = "Incoming attacks must exceed the block value to deal damage.";
        }

        if(card.ability[0].chilled != null){
            Title.color = Color.blue;
            Title.text = "Chilled";
            Description.text = "Increases damage taken by 50%";
        }

        if(card.ability[0].invisible != null){
            Title.color = Color.yellow;
           Title.text = "Invisible";
           Description.text = "Unable to be targeted, at the start of a invisible players turn the invisible stack decreases by 1";
        }

        if(card.ability[0].retreat != null){
            Title.color = Color.red;
            Title.text = "Retreat";
            Description.text = "Reposition Caster to the back of the party";
        }

        if(card.ability[0].heal != null){
            Title.color = Color.yellow;
            Title.text = "Heal";
            Description.text = "Heals Caster for X ammount";
        }

        if(card.ability[0].healParty != null){
            Title.color = Color.yellow;
            Title.text = "Heal";
            Description.text = "Heals Party for X ammount";
        }        
    }

}
