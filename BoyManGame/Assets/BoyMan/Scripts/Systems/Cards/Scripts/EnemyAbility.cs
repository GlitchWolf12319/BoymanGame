using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

[Serializable]
public class EnemyAbility : FindTargets
{
    public void Pull(GameObject target, GameObject target2){
            Vector3 target1Pos = target.transform.position;
            target.transform.DOMoveX(target2.transform.position.x, 0.5f);
            target2.transform.DOMoveX(target1Pos.x, 0.5f);
    }

    public void Push(GameObject target, GameObject target2){
            Vector3 target1Pos = target.transform.position;
            target.transform.DOMoveX(target2.transform.position.x, 0.5f);
            target2.transform.DOMoveX(target1Pos.x, 0.5f);
    }

    public void Retreat(GameObject target, GameObject target2){
        Vector3 target1Pos = target.transform.position;
        target.transform.DOMoveX(target2.transform.position.x, 0.5f);
        target2.transform.DOMoveX(target1Pos.x, 0.5f);
    }

    public void GiveGuard(GameObject target, int ammount, GameObject shieldIconPrefab){

        GameObject shield = Instantiate(shieldIconPrefab, transform.position, Quaternion.identity);

        target.GetComponent<CharacterController>().GainGuard(ammount);
    }

    public void Heal(GameObject target, int ammount){

            int healAmmount = ammount;
            target.GetComponent<CharacterController>().Heal(healAmmount);
    }

    public void DealDamage(GameObject target, int ammount){
            target.GetComponent<CharacterController>().TakeDamage(ammount, "NormalDamage");
    }

    public void Ignite(GameObject target, int ammount, int stack){

            target.GetComponent<CharacterController>().igniteAmmount = ammount;
            target.GetComponent<CharacterController>().igniteStack += stack;
    }

    public void Poison(GameObject target, int ammount, int stack){

            target.GetComponent<CharacterController>().poisonAmmount = ammount;
            target.GetComponent<CharacterController>().poisonStack += stack;
    }

    public void Chilled(GameObject target, int stack){
        target.GetComponent<CharacterController>().chilledStack += stack;
    }

    public void Invisible(GameObject target, int stack){
        target.GetComponent<CharacterController>().invisibleStack = stack;
        target.GetComponent<CharacterController>().Invisible();
    }

   public void DealPartyDamage(int ammount){
            List<GameObject> targets = FindEnemies();

            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().TakeDamage(ammount, "NormalDamage");
            }
    }
   
   public void IgniteAllEnemies(int ammount, int stack){
            List<GameObject> targets = FindEnemies();
            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().igniteAmmount = ammount;
                targets[i].GetComponent<CharacterController>().igniteStack += stack;
            }
    }

   public void HealParty(int ammount){
            Debug.Log("Healing Party");
            List<GameObject> targets = FindGoodChar();

            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().Heal(ammount);
            }
   }
}
