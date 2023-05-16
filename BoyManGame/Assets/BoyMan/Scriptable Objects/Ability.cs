using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Card", menuName = "CardAbility/Ability")]
public class Ability : ScriptableObject
{
    public enum CardAbility { Damage, Ignite, Poison }  // Enum representing different card abilities
    public CardAbility cardAbility;  // Selected card ability

    [Header("Damage Ability")]
    public int damageAmount;  // Amount of damage for the damage ability

    [Header("Ignite Ability")]
    public int igniteAmount;  // Amount of ignition for the ignite ability

    [Header("Poison Ability")]
    public int poisonAmount;  // Amount of poison for the poison ability

    public enum Target { Self, Enemy, AllEnemies }  // Enum representing different target options
    public Target target;  // Selected target option

    public List<GameObject> ReturnTargets()
    {
        List<GameObject> targetsInScene = new List<GameObject>();

        if (target == Target.Enemy)
        {
            // Find all game objects with the "Enemy" tag and add them to the targets list
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (enemy != null)
                {
                    targetsInScene.Add(enemy);
                }
            }
        }

        return targetsInScene;
    }
}