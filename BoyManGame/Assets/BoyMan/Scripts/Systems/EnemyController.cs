using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [SerializeField] CharacterStats CS;
    [SerializeField] private string CharName;
    [SerializeField] private int health;
    [SerializeField] private int MaxHealth;

    [Header("Status Effects")]
    public int igniteStack;
    public int poisonStack;
    // Start is called before the first frame update
    void Start()
    {
        AssignStats();
    }

    void AssignStats(){
        MaxHealth = CS.MaxHealth;
        health = MaxHealth;
        CharName = CS.CharacterName;
    }

    public void TakeDamage(int ammount){
        health -= ammount;
        if(health <= 0){
            health = 0;
            Die();
        }
    }

    void Die(){
        transform.DOScale(new Vector3(0,0,0), 0.5f);
        Destroy(this.gameObject, 0.7f);
    }

    public void CheckStatusEffects(){
        TakeDamage(IgniteDamage(5));
        TakeDamage(poisonDamage(8));
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            CheckStatusEffects();
        }
    }

    public int IgniteDamage(int damage){
        igniteStack--;

        if(igniteStack >= 0){
            return damage;
        }
        else{
            igniteStack = 0;
            return 0;
        }
    }
    
    public int poisonDamage(int damage){
        poisonStack--;

        if(poisonStack >= 0){
            return damage;
        }
        else{
            poisonStack = 0;
            return 0;
        }
    }
}
