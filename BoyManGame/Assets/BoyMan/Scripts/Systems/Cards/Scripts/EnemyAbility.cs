using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

[Serializable]
public class EnemyAbility : FindTargets
{
public void Pull(GameObject target, GameObject target2)
{
    // Check if the character has a guard
    if (transform.GetComponent<CharacterController>().guard > 0)
    {
        transform.GetComponent<CharacterController>().guard = 0;
        // Instantiate a broken shield effect
        Instantiate(transform.GetComponent<CharTurn>().brokenShield, transform.position, Quaternion.identity);
    }

    Vector3 target1Pos = target.transform.position;
    // Move the first target towards the position of the second target using a Tween animation
    target.transform.DOMoveX(target2.transform.position.x, 0.5f);
    // Move the second target towards the position of the first target using a Tween animation
    target2.transform.DOMoveX(target1Pos.x, 0.5f);
}

public void Push(GameObject target, GameObject target2)
{
    // Check if the character has a guard
    if (transform.GetComponent<CharacterController>().guard > 0)
    {
        transform.GetComponent<CharacterController>().guard = 0;
        // Instantiate a broken shield effect
        Instantiate(transform.GetComponent<CharTurn>().brokenShield, transform.position, Quaternion.identity);
    }

    Vector3 target1Pos = target.transform.position;
    // Move the first target towards the position of the second target using a Tween animation
    target.transform.DOMoveX(target2.transform.position.x, 0.5f);
    // Move the second target towards the position of the first target using a Tween animation
    target2.transform.DOMoveX(target1Pos.x, 0.5f);
}

public void Retreat(GameObject target, GameObject target2)
{
    // Check if the character has a guard
    if (transform.GetComponent<CharacterController>().guard > 0)
    {
        transform.GetComponent<CharacterController>().guard = 0;
        // Instantiate a broken shield effect
        Instantiate(transform.GetComponent<CharTurn>().brokenShield, transform.position, Quaternion.identity);
    }

    Vector3 target1Pos = target.transform.position;
    // Move the first target towards the position of the second target using a Tween animation
    target.transform.DOMoveX(target2.transform.position.x, 0.5f);
    // Move the second target towards the position of the first target using a Tween animation
    target2.transform.DOMoveX(target1Pos.x, 0.5f);
}

public void GiveGuard(GameObject target, int amount, GameObject shieldIconPrefab)
{
    // Check if the character has a guard
    if (transform.GetComponent<CharacterController>().guard > 0)
    {
        transform.GetComponent<CharacterController>().guard = 0;
        // Instantiate a broken shield effect
        Instantiate(transform.GetComponent<CharTurn>().brokenShield, transform.position, Quaternion.identity);
    }

    // Instantiate a shield icon at the character's position
    GameObject shield = Instantiate(shieldIconPrefab, transform.position, Quaternion.identity);

    // Increase the guard of the target character
    target.GetComponent<CharacterController>().GainGuard(amount);
}

public void Heal(GameObject target, int amount)
{
    // Check if the character has a guard
    if (transform.GetComponent<CharacterController>().guard > 0)
    {
        transform.GetComponent<CharacterController>().guard = 0;
        // Instantiate a broken shield effect
        Instantiate(transform.GetComponent<CharTurn>().brokenShield, transform.position, Quaternion.identity);
    }

    int healAmount = amount;
    // Heal the target character
    target.GetComponent<CharacterController>().Heal(healAmount);
}
    public void DealDamage(GameObject target, int ammount){
          // Check if the character has a guard

        if(transform.GetComponent<CharacterController>().guard > 0){
                transform.GetComponent<CharacterController>().guard = 0;
                  // Instantiate a broken shield effect
                Instantiate(transform.GetComponent<CharTurn>().brokenShield, transform.position, Quaternion.identity);
        }

            TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();
            if(tbm.turns[tbm.turnCounter].GetComponent<CharacterController>().hasWeaken()){
                Debug.Log("Has Weaken");
                float quat = ammount * 0.25f;
                float newDamage = ammount - quat;
                int intDamage = Mathf.RoundToInt(newDamage);
                Debug.Log("Weaken Damage " + intDamage);
// Deal reduced damage to the target character if it has the "Weaken" status effect
                target.GetComponent<CharacterController>().TakeDamage(intDamage, "NormalDamage");    
            }
            else{
                // Deal normal damage to the target character
                target.GetComponent<CharacterController>().TakeDamage(ammount, "NormalDamage");
                Debug.Log("Dealing Damage");
            }
            
    }

    public void Ignite(GameObject target, int ammount, int stack){
  // Check if the character has a guard
        if(transform.GetComponent<CharacterController>().guard > 0){
            
                transform.GetComponent<CharacterController>().guard = 0;
                    // Instantiate a broken shield effect
                Instantiate(transform.GetComponent<CharTurn>().brokenShield, transform.position, Quaternion.identity);
        }

            
            if(target.GetComponent<CharacterController>().guard < stack){
                  // Apply the "Ignite" status effect to the target character
                target.GetComponent<CharacterController>().igniteAmmount = ammount;
                target.GetComponent<CharacterController>().igniteStack += stack;
            }
            
    }

    public void Poison(GameObject target, int ammount, int stack){
 // Check if the character has a guard
        if(transform.GetComponent<CharacterController>().guard > 0){
                transform.GetComponent<CharacterController>().guard = 0;
                  // Instantiate a broken shield effect
                Instantiate(transform.GetComponent<CharTurn>().brokenShield, transform.position, Quaternion.identity);
        }

            if(target.GetComponent<CharacterController>().guard < stack){
                // Apply the "Poison" status effect to the target character
                target.GetComponent<CharacterController>().poisonAmmount = ammount;
                target.GetComponent<CharacterController>().poisonStack += stack;
            }
            
    }

    public void Chilled(GameObject target, int stack){
 // Check if the character has a guard
        if(transform.GetComponent<CharacterController>().guard > 0){
                transform.GetComponent<CharacterController>().guard = 0;
                // Instantiate a broken shield effect
                Instantiate(transform.GetComponent<CharTurn>().brokenShield, transform.position, Quaternion.identity);
        }

        if(target.GetComponent<CharacterController>().guard < stack){
             // Increase the "Chilled" stack of the target character
            target.GetComponent<CharacterController>().chilledStack += stack;
        }
        
    }

    public void Invisible(GameObject target, int stack){
// Check if the character has a guard
        if(transform.GetComponent<CharacterController>().guard > 0){
                transform.GetComponent<CharacterController>().guard = 0;
                 // Instantiate a broken shield effect
                Instantiate(transform.GetComponent<CharTurn>().brokenShield, transform.position, Quaternion.identity);
        }
 // Set the "Invisible" stack of the target character
        target.GetComponent<CharacterController>().invisibleStack = stack;
        // Make the target character invisible
        target.GetComponent<CharacterController>().Invisible();
    }

public void DealPartyDamage(int amount)
{
    // Find all enemy characters
    List<GameObject> targets = FindEnemies();

    // Deal damage to each enemy character in the list
    for (int i = 0; i < targets.Count; i++)
    {
        targets[i].GetComponent<CharacterController>().TakeDamage(amount, "NormalDamage");
    }
}

public void IgniteAllEnemies(int amount, int stack)
{
    // Find all enemy characters
    List<GameObject> targets = FindEnemies();

    // Apply the "Ignite" status effect to each enemy character in the list
    for (int i = 0; i < targets.Count; i++)
    {
        if (targets[i].GetComponent<CharacterController>().guard < stack)
        {
            targets[i].GetComponent<CharacterController>().igniteAmmount = amount;
            targets[i].GetComponent<CharacterController>().igniteStack += stack;
        }
    }
}

public void HealParty(int amount)
{
    Debug.Log("Healing Party");

    // Find all friendly characters
    List<GameObject> targets = FindGoodChar();

    // Heal each friendly character in the list
    for (int i = 0; i < targets.Count; i++)
    {
        targets[i].GetComponent<CharacterController>().Heal(amount);
    }
}
}
