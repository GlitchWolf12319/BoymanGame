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
    [SerializeField] private int Coins;

    [Header("Status Effects")]
    public int igniteStack;
    public int igniteAmmount;
    public int poisonStack;
    public int poisonAmmount;
    public int chilledStack;
    public int invisibleStack;
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

    public void GiveCoins(int ammount){
        Coins += ammount;
    }

    public void Invisible(){
        Color spriteColour = transform.GetComponent<SpriteRenderer>().color;
        spriteColour.a = 0.2f;
        transform.GetComponent<SpriteRenderer>().color = spriteColour;
    }

    public void Visible(){
        Color spriteColour = transform.GetComponent<SpriteRenderer>().color;
        spriteColour.a = 1f;
        transform.GetComponent<SpriteRenderer>().color = spriteColour;
    }

    public void CheckStatusEffects(){
        TakeDamage(IgniteDamage(5));
        TakeDamage(poisonDamage(poisonAmmount));
        hasChilled();

        if(!isInvisible()){
            Visible();
        }
    }

    public void RemoveNegativeEffects(){
        igniteStack = 0;
        igniteAmmount = 0;
        poisonStack = 0;
        poisonAmmount = 0;
        chilledStack = 0;
        invisibleStack = 0;
        Visible();
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

    public bool isInvisible(){

        if(invisibleStack > 0){
            invisibleStack--;
            return true;
        }
        else{
            invisibleStack = 0;
            return false;
        }
    }

}
