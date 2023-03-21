using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Card : FindTargets
{
    public CardTemplate card;
    public bool selected;
    public Vector3 originalPosition;
    public int index;
    public Vector3 cardRotation;
    private List<Card> CloseCards = new List<Card>();
    private List<Card> FarCards = new List<Card>();
    public bool canSelectTarget;
    public List<GameObject> targets = new List<GameObject>();
    private bool cardPlayed = false;
    [SerializeField]public bool onHover = false;
    [SerializeField]private bool Drag = false;
    public bool Arrow = false;
    public bool canHover;
    [SerializeField] GameObject caster;
    public GameObject arrow;
    public GameObject speechBubblePrefab;
    public Color canUseColor;
    public Color cantUseColor;

    public void AssingCaster(){
        Transform CardsParent = this.transform.parent;
        caster = CardsParent.parent.gameObject;
    }

    void Update(){
        if(canSelectTarget){
            SelectTarget();
        }

        if(Input.GetMouseButtonDown(0) && onHover){
            selected = true;
            canSelectTarget = true;
            CollectTarget();
            if(card.attackMethod == CardTemplate.AttackMethod.Drag){
                if(caster.GetComponent<CharTurn>().ActionPoints >= card.APCost){
                    Drag = true;
                }
                else{
                    StartCoroutine(cantUseCard());
                }
            }

            if(card.attackMethod == CardTemplate.AttackMethod.Arrow){
                if(caster.GetComponent<CharTurn>().ActionPoints >= card.APCost){
                    Arrow = true;
                }
                else{
                    StartCoroutine(cantUseCard());
                }
                
            }
        }
        
        if(Input.GetMouseButtonUp(0)){

            if(card.attackMethod == CardTemplate.AttackMethod.Drag){
                Drag = false;
                CheckIfCardIsPlayed();
            }
        }

        if(Input.GetMouseButtonDown(1) && onHover){
            selected = false;
            canSelectTarget = false;
            if(card.attackMethod == CardTemplate.AttackMethod.Drag){
                Drag = false;
                transform.DOMove(originalPosition, 0.5f);
            }

            if(card.attackMethod == CardTemplate.AttackMethod.Arrow){
                Arrow = false;
            }
            
        }

        if(Drag){
            MoveCardByMouse();
        }

        if(Arrow){
            arrow.SetActive(true);
        }
        else{
            arrow.SetActive(false);
        }
        
    }

    IEnumerator cantUseCard(){
        Vector3 upPos = new Vector3(0, originalPosition.y + 200, 0);
        Vector3 downPos = new Vector3(0,upPos.y - 100 , 0);

        transform.DOMoveY(upPos.y, 0.5f);
        yield return new WaitForSeconds(0.2f);
        transform.DOMoveY(downPos.y, 0.5f);

        GameObject bubble = Instantiate(speechBubblePrefab, caster.transform.position, Quaternion.identity);
        bubble.transform.position = new Vector3(bubble.transform.position.x + 1, bubble.transform.position.y + 2, bubble.transform.position.z);
        Destroy(bubble, 5);
    }

    public void CheckCurrentAPAgainstCard(){

        if(card.APCost > caster.GetComponent<CharTurn>().ActionPoints){
            foreach(Transform children in this.transform){
                if(children.name.Contains("Outline")){
                    Image ImageOutline = children.GetComponent<Image>();
                    ImageOutline.color = cantUseColor;
                }
            }

            this.GetComponent<CardRender>().APCost.color = cantUseColor;
        }
        else if(card.APCost <= caster.GetComponent<CharTurn>().ActionPoints){
            foreach(Transform children in this.transform){
                if(children.name.Contains("Outline")){
                    Image ImageOutline = children.GetComponent<Image>();
                    ImageOutline.color = canUseColor;
                }
            }

            this.GetComponent<CardRender>().APCost.color = Color.white;
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
                Debug.Log("hit " + hit.transform.name);
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
            caster.GetComponent<CharTurn>().GainCardInfo(card, this, target, 0,  card.ability[i].dealDamage.damageAmmount, i, card.APCost);
        }

        if(card.ability[i].dealPartyDamage != null){
            caster.GetComponent<CharTurn>().GainCardInfo(card, this, null, 0,  card.ability[i].dealPartyDamage.damageAmmount, i, card.APCost);
        }

        if(card.ability[i].igniteEffect != null){
            caster.GetComponent<CharTurn>().GainCardInfo(card, this, target, card.ability[i].igniteEffect.IgniteStack, card.ability[i].igniteEffect.IgniteAmmount, i, card.APCost);
        }

        if(card.ability[i].poisonEffect != null){
            caster.GetComponent<CharTurn>().GainCardInfo(card, this, target, card.ability[i].poisonEffect.poisonStack, card.ability[i].poisonEffect.poisonAmmount, i, card.APCost);
        }

        if(card.ability[i].guard != null){
            target = caster;
            caster.GetComponent<CharTurn>().GainCardInfo(card, this, target, 0, card.ability[i].guard.guardAmmount, i, card.APCost);
        }

        if(card.ability[i].heal != null){
            target = caster;
            target.GetComponent<CharTurn>().GainCardInfo(card, this, target, 0, card.ability[i].heal.healAmmount, i, card.APCost);
        }

        if(card.ability[i].healParty != null){
            caster.GetComponent<CharTurn>().GainCardInfo(card, this, null, 0,  card.ability[i].healParty.healAmmount, i, card.APCost);
        }

        if(card.ability[i].chilled != null){
            target.GetComponent<CharTurn>().GainCardInfo(card, this, target, card.ability[i].chilled.chilledStack, 0, i, card.APCost);
        }

        if(card.ability[i].invisible != null){
            target = caster;
            target.GetComponent<CharTurn>().GainCardInfo(card, this, target, card.ability[i].invisible.invisibleStack, 0, i, card.APCost);
        }

        if(card.ability[i].retreat != null){
            target = caster;
            target.GetComponent<CharTurn>().GainCardInfo(card, this,  target, 0, 0, i, card.APCost);
        }

        if(card.ability[i].igniteParty != null){
            target = caster;
            target.GetComponent<CharTurn>().GainCardInfo(card, this, null, card.ability[i].igniteParty.IgniteStack, card.ability[i].igniteParty.IgniteAmmount, i, card.APCost);
        }

        DeckDrawing dd = caster.GetComponent<DeckDrawing>();
        for(int c = 0; c < dd.hand.Count; c++){
            dd.hand[c].transform.DOMove(dd.hand[c].originalPosition, 0.5f);
        }

        targets.Clear();

    }
    }

    public void Hover(){

        if(!Drag && !cardPlayed && canHover && !Arrow){

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
        if(!Drag && !cardPlayed && canHover && !Arrow){
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
