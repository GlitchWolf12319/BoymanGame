using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : FindTargets
{
    public CardTemplate card;
    [SerializeField]private bool selected;
    public Vector3 originalPosition;
    public int index;
    public Vector3 cardRotation;
    private List<Card> CloseCards = new List<Card>();
    private List<Card> FarCards = new List<Card>();
    private bool canSelectTarget;
    [SerializeField]private List<GameObject> targets = new List<GameObject>();
    private bool cardPlayed = false;
    [SerializeField]private bool onHover = false;
    [SerializeField]private bool Drag = false;
    public bool canHover;
    [SerializeField] GameObject caster;
    
    public void CardSelected(){
        if(selected){
            selected = false;
            canSelectTarget = false;
        }
        else{
            selected = true;
            canSelectTarget = true;
            CollectTarget();
        }
    }

    void Update(){
        if(canSelectTarget){
            SelectTarget();
        }

        if(Input.GetMouseButtonDown(0) && onHover){
            if(card.attackMethod == CardTemplate.AttackMethod.Drag){
                Drag = true;
            }
        }
        
        if(Input.GetMouseButtonUp(0)){

            if(card.attackMethod == CardTemplate.AttackMethod.Drag){
                Drag = false;
                CheckIfCardIsPlayed();
            }
        }

        if(Input.GetMouseButtonDown(1) && onHover){
            if(card.attackMethod == CardTemplate.AttackMethod.Drag){
                Drag = false;
                transform.DOMove(originalPosition, 0.5f);
            }
            
        }

        if(Drag){
            MoveCardByMouse();
        }
        
    }

    void CheckIfCardIsPlayed(){
        float YPos = originalPosition.y + 500;

        if(transform.position.y > YPos){
            CheckAbility(this.gameObject);
            DeckDrawing dd = caster.GetComponent<DeckDrawing>();
            selected = false;
            Drag = false;
            onHover = false;
            cardPlayed = true;
        }
    }

    void MoveCardByMouse(){
        transform.position = Input.mousePosition;
    }

    public void CollectTarget(){
        targets = FindEnemies();
    }

    public void SelectTarget(){
        
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Perform the raycast and get the hit information
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100)){

                for(int i = 0; i < targets.Count; i++){
                    if(hit.transform.name == targets[i].name){
                        CheckAbility(hit.transform.gameObject);
                    }
                }    
            
            }
        }
    }

    void CheckAbility(GameObject target){
        for(int i = 0; i < card.ability.Length; i++){

        if(card.ability[i].dealDamage != null){
            caster.GetComponent<CharTurn>().GainCardInfo(card, target, 0,  card.ability[i].dealDamage.damageAmmount, i);
        }

        if(card.ability[i].dealPartyDamage != null){
            caster.GetComponent<CharTurn>().GainCardInfo(card, null, 0,  card.ability[i].dealPartyDamage.damageAmmount, i);
        }

        if(card.ability[i].igniteEffect != null){
            caster.GetComponent<CharTurn>().GainCardInfo(card, target, card.ability[i].igniteEffect.IgniteStack, card.ability[i].igniteEffect.IgniteAmmount, i);
        }

        if(card.ability[i].poisonEffect != null){
            caster.GetComponent<CharTurn>().GainCardInfo(card, target, card.ability[i].poisonEffect.poisonStack, card.ability[i].poisonEffect.poisonAmmount, i);
        }

        if(card.ability[i].guard != null){
            target = caster;
            caster.GetComponent<CharTurn>().GainCardInfo(card, target, 0, card.ability[i].guard.guardAmmount, i);
        }

        if(card.ability[i].heal != null){
            target = caster;
            target.GetComponent<CharTurn>().GainCardInfo(card, target, 0, card.ability[i].heal.healAmmount, i);
        }

        if(card.ability[i].healParty != null){
            caster.GetComponent<CharTurn>().GainCardInfo(card, null, 0,  card.ability[i].healParty.healAmmount, i);
        }

        if(card.ability[i].chilled != null){
            target.GetComponent<CharTurn>().GainCardInfo(card, target, card.ability[i].chilled.chilledStack, 0, i);
        }

        if(card.ability[i].invisible != null){
            target = caster;
            target.GetComponent<CharTurn>().GainCardInfo(card, target, card.ability[i].invisible.invisibleStack, 0, i);
        }

        if(card.ability[i].retreat != null){
            target = caster;
            target.GetComponent<CharTurn>().GainCardInfo(card, target, 0, 0, i);
        }

        if(card.ability[i].igniteParty != null){
            target = caster;
            target.GetComponent<CharTurn>().GainCardInfo(card, null, card.ability[i].igniteParty.IgniteStack, card.ability[i].igniteParty.IgniteAmmount, i);
        }

        targets.Clear();

    }

        DeckDrawing dd = caster.GetComponent<DeckDrawing>();
        canSelectTarget = false;
        targets.Clear();
        dd.StartCoroutine(dd.MoveToDiscardPile(this));
    }

    public void Hover(){

        if(!Drag && !cardPlayed && canHover){

        DeckDrawing dd = caster.GetComponent<DeckDrawing>();
        onHover = true;

        cardRotation = transform.eulerAngles;
        transform.DOScale(new Vector3(1.5f,1.5f,1.5f), 0.5f);
        transform.DOMoveY(transform.position.y + 100, 0.5f);
        transform.DORotate(new Vector3(0,0,0), 0.5f);
        int LastIndex = dd.hand.Count - 1;

        if(dd.hand[0].transform.name == transform.name){
            dd.hand[0].transform.DOMoveY(transform.position.y + 225, 0.5f);
        }
        if(dd.hand[LastIndex].transform.name == transform.name){
            dd.hand[LastIndex].transform.DOMoveY(transform.position.y + 225, 0.5f);
        }

        for(int i = 0; i < dd.hand.Count; i++){

            CloseCards.Add(dd.hand[i]);
            if(dd.hand[i].index < index - 1 || dd.hand[i].index > index + 1){
                CloseCards.Remove(dd.hand[i]);
                FarCards.Add(dd.hand[i]);
                CloseCards.Remove(this);
            }
        }

        
        for(int i = 0; i < CloseCards.Count; i++){
            float[] Closeoffset = {CloseCards[i].originalPosition.x + 120, CloseCards[i].originalPosition.x - 120};
            if(index < CloseCards[i].index){
                CloseCards[i].transform.DOMoveX(Closeoffset[0], 0.5f);
            }
            else if(index > CloseCards[i].index){
                CloseCards[i].transform.DOMoveX(Closeoffset[1], 0.5f);
            }
        }

        for(int i = 0; i < FarCards.Count; i++){
            float[] Faroffset = {FarCards[i].originalPosition.x + 100, FarCards[i].originalPosition.x - 100};
            if(index < FarCards[i].index){
                FarCards[i].transform.DOMoveX(Faroffset[0], 0.5f);
            }
            else if(index > FarCards[i].index){
                FarCards[i].transform.DOMoveX(Faroffset[1], 0.5f);
            }
        }

        }
    
    }

    public void NoHover(){
        if(!Drag && !cardPlayed && canHover){
        onHover = false;
        transform.DOScale(new Vector3(1.03905f, 1.03905f, 1.03905f), 0.5f);
        transform.DOMove(originalPosition, 0.5f);
        transform.DORotate(cardRotation, 0.5f);
        CloseCards.Clear();
        FarCards.Clear();

        DeckDrawing dd = caster.GetComponent<DeckDrawing>();
        for(int i = 0; i < dd.hand.Count; i++){
            dd.hand[i].transform.DOMove(dd.hand[i].originalPosition, 0.5f);
        }

    }

    }

}
