using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Card", menuName = "CardAbility/Ability")]
public class Ability : ScriptableObject
{
        public enum CardAbility {Damage, Ignite, Poison}
        public CardAbility cardAbility;

        [Header("Damage Ability")]
        public int damageAmmount;

        [Header("Ignite Ability")]
        public int igniteAmmount;
        [Header("Poison Ability")]
        public int poisonAmmount;

        public enum target {Self, Enemy, allEnemies}
        public target Target;

        public List<GameObject> ReturnTargets(){
                List<GameObject> targetsInScene = new List<GameObject>();
                if(Target == target.Enemy){
                        foreach(var enemy in GameObject.FindGameObjectsWithTag("Enemy")){
                        if(enemy != null){
                                targetsInScene.Add(enemy);
                        }
                        }
                }
                return targetsInScene;
        }
}      
