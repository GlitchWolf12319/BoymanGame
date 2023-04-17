using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class CharacterController : FindTargets
{
    public CharacterStats CS;
    [SerializeField] private string CharName;
    public int health;
    public int MaxHealth;
    public int guard;
    [SerializeField] private int Coins;
    public Animator anim;
    [SerializeField] private GameObject damageTextPrefab;
    public bool dead;

    public AudioSource audioPlayer;

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

    public void OnMouseEnter() {

        if(transform.tag == "Enemy"){
        GameObject arrow = GameObject.FindGameObjectWithTag("Arrow");
        List<Transform> arrowParts = new List<Transform>();
        if(arrow != null){

            foreach(Transform children in arrow.transform){
                arrowParts.Add(children);
            }

            for(int i = 0; i < arrowParts.Count; i++){
                arrowParts[i].GetComponent<Image>().color = Color.blue;
            }

            }
        }
    }

    public void OnMouseExit() {

        if(transform.tag == "Enemy"){
        GameObject arrow = GameObject.FindGameObjectWithTag("Arrow");
        List<Transform> arrowParts = new List<Transform>();
        if(arrow != null){

            foreach(Transform children in arrow.transform){
                arrowParts.Add(children);
            }

            for(int i = 0; i < arrowParts.Count; i++){
                arrowParts[i].GetComponent<Image>().color = Color.white;
            }

            }
        }
    }

    IEnumerator DamageColor(){
        if(transform.tag == "Enemy"){
            this.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void TakeDamage(int ammount, string DamageType){
        Debug.Log("targets name " + transform.name + " targets guard before attack " + guard);
        //IF PLAYER HAS MORE GUARD THAN DAMAGE BEING TAKEN
        if(guard >= ammount){
            guard -= ammount;
            if(guard <= 0){
                guard = 0;
            }
        }

        //IF PLAYER HAS LESS GUARD THAN DAMAGE BEING TAKEN
        if(guard < ammount){
            int remainingDamage = ammount - guard;
            health -= remainingDamage;
            guard -= ammount;
        }
        
        Debug.Log("targets guard after attack " + guard);
        DamageIdicator damageIdicator = Instantiate(damageTextPrefab, transform.position, Quaternion.identity).GetComponent<DamageIdicator>();

        if(ammount > 0 && guard < ammount){
            damageIdicator.SetDamageText(ammount);
            StartCoroutine(DamageColor());
        }

        if(ammount > 0 && guard >= ammount){
            damageIdicator.SetDamageText(0);
            StartCoroutine(DamageColor());
        }

        damageIdicator.SetDamageColor(Color.red);

        if(health <= 0 && !dead){
            health = 0;
            Die(DamageType);
        }

        if(hasChilled()){
            ammount *= 2;
        }
        else{
            ammount = ammount;
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

    void Die(string DamageType){
        transform.DOScale(new Vector3(0,0,0), 0.5f);


        TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();
        if(tbm.battleInProgress && !dead){
            GameObject.Find("RestartScreen").GetComponent<Restart>().removeTarget(this.gameObject);
            transform.GetComponent<CharTurn>().tbm.RemoveFromTurnOrder(this.gameObject, DamageType);
        }

        dead = true;

        GameObject jane = GameObject.Find("Jane");
        GameObject boyman = GameObject.Find("BoyMan");
        if(jane != null){
            if(jane.GetComponent<CharacterController>().dead){
                Camera mainCam = Camera.main;
                if(boyman != null){
                    mainCam.GetComponent<CameraFollow>().player = GameObject.Find("BoyCamFollow").transform;
                }
                
            }
        }

        if(boyman != null){
            if(boyman.GetComponent<CharacterController>().dead){
                if(jane != null && jane.GetComponent<CharacterController>().dead == false){
                    Vector3 newPos = transform.position;
                    jane.transform.DOMove(newPos, 0.5f);
                }
            }
        }
        
        Destroy(this.gameObject, 10f);


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
        Color spriteColour = transform.GetComponentInChildren<SpriteRenderer>().color;
        spriteColour.a = 1f;
        transform.GetComponentInChildren<SpriteRenderer>().color = spriteColour;
    }

    public void CheckStatusEffects(){

        if(health > 0){

        if(IgniteDamage(igniteStack) != 0){
            TakeDamage(igniteStack + 1, "Ignite Damage");
        }

        if(poisonDamage(poisonStack) != 0){
            TakeDamage(poisonStack + 1, "Poison Damage");
        }
        }
        
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
