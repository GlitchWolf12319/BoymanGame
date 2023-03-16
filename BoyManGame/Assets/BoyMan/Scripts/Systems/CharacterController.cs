using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    [SerializeField] CharacterStats CS;
    [SerializeField] private string CharName;
    [SerializeField] private int health;
    [SerializeField] private int MaxHealth;
    [SerializeField] private int guard;

    [Header("Status Effects")]
    public int igniteStack;
    public int igniteAmmount;
    public int poisonStack;
    public int poisonAmmount;
    public int chilledStack;
    // Start is called before the first frame update
    void Awake()
    {
        AssignStats();
    }

    void AssignStats(){
        MaxHealth = CS.MaxHealth;
        health = MaxHealth;
        CharName = CS.CharacterName;
    }

    public void TakeDamage(int ammount){
        if(hasChilled()){
            ammount *= 2;
        }
        else{
            ammount = ammount;
        }

        health -= ammount;
        if(health <= 0){
            health = 0;
            Die();
        }
    }

    public void Heal(int ammount){
        health += ammount;
        if(health >= MaxHealth){
            health = MaxHealth;
        }
    }

    public void GainGuard(int ammount){
        guard += ammount;
    }

    void Die(){
        transform.DOScale(new Vector3(0,0,0), 0.5f);
        transform.GetComponent<CharTurn>().tbm.RemoveFromTurnOrder(this.gameObject);
        Destroy(this.gameObject, 0.7f);
    }

    public void CheckStatusEffects(){
        TakeDamage(IgniteDamage(5));
        TakeDamage(poisonDamage(poisonAmmount));
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

    public bool hasChilled(){

        if(chilledStack > 0){
            chilledStack--;
            return true;
        }
        else{
            chilledStack = 0;
            return false;
        }
    }
}
