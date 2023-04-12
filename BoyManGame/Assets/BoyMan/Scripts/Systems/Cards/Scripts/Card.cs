using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;


public class Card : FindTargets
{
    //collect information from card template
    public CardTemplate card;

    //enabled / disabled based on if card is selected
    public bool selected;

    //original position of the card;
    public Vector3 originalPosition;

    //index of the card
    public int index;

    //cards rotation
    public Vector3 cardRotation;

    //cards close to card selected
    public List<GameObject> CloseCards = new List<GameObject>();
    
    //cards far from card selected
    public List<GameObject> FarCards = new List<GameObject>();

    //checks if card can select targets
    public bool canSelectTarget;

    //targets who can be attacked
    public List<GameObject> targets = new List<GameObject>();

    //enables / disables based on if mouse is over card
    [SerializeField]public bool onHover = false;

    //checks if card is dragable
    public bool Drag = false;

    //check if card is arrow
    public bool Arrow = false;

    //checks if card can be hovered over
    public bool canHover;

    //caster of the card
    [SerializeField]public GameObject caster;

    public GameObject shieldIconPrefab;

    public GameObject arrow;
    public GameObject speechBubblePrefab;
    public Color canUseColor;
    public Color cantUseColor;
    public bool canSelect;
    public bool disableHovering;



    public void CollectTarget(){
        targets = FindEnemies();
    }

    public void SelectTarget(){
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
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
            DealDamage(target, card.ability[i].dealDamage.damageAmmount);
            //StartCoroutine(CameraAttack(target, false));
        }

        if(card.ability[i].dealPartyDamage != null){
            DealPartyDamage(card.ability[i].dealPartyDamage.damageAmmount);
            //StartCoroutine(CameraAttack(target, false));
        }

        if(card.ability[i].igniteEffect != null){
            Ignite(target, card.ability[i].igniteEffect.IgniteStack, card.ability[i].igniteEffect.IgniteStack);
            //StartCoroutine(CameraAttack(target, false));
        }

        if(card.ability[i].poisonEffect != null){
            Poison(target, card.ability[i].poisonEffect.poisonStack, card.ability[i].poisonEffect.poisonStack);
            //StartCoroutine(CameraAttack(target, false));
        }

        if(card.ability[i].guard != null){
            target = caster;
            GiveGuard(target, card.ability[i].guard.guardAmmount);
            //StartCoroutine(CameraAttack(target, true));
        }

        if(card.ability[i].heal != null){
            target = caster;
            Heal(target, card.ability[i].heal.healAmmount);
            //StartCoroutine(CameraAttack(target, true));
        }

        if(card.ability[i].healParty != null){
            HealParty(card.ability[i].healParty.healAmmount);
            //StartCoroutine(CameraAttack(target, true));
        }

        if(card.ability[i].chilled != null){
            Chilled(target, card.ability[i].chilled.chilledStack);
            // StartCoroutine(CameraAttack(target, false));
        }

        if(card.ability[i].invisible != null){
            target = caster;
            Invisible(target, card.ability[i].invisible.invisibleStack);
        }

        if(card.ability[i].retreat != null){
            List<GameObject> possibleTargets = FindGoodChar();
                if(possibleTargets != null){
                    GameObject RetreatTarget = null;
                    while(RetreatTarget == null){
                        int randomChoice = Random.Range(0, possibleTargets.Count);
                        if(possibleTargets[randomChoice].transform.name != this.transform.name){
                            RetreatTarget = possibleTargets[randomChoice];
                        }
                    }

                    if(RetreatTarget != null){
                        Retreat(target, RetreatTarget);
                        //StartCoroutine(CameraAttack(target, false));
                        possibleTargets.Clear();
                     }
                }
        }

        if(card.ability[i].igniteParty != null){
            IgniteAllEnemies(card.ability[i].igniteParty.IgniteStack, card.ability[i].igniteParty.IgniteStack);
            //StartCoroutine(CameraAttack(target, false));
        }

        targets.Clear();
    }

        Debug.Log("target name: " + target.name + " Target Tag: " + target.tag);
        if(target.tag != "Player" || card.ability.Length > 1){
            StartCoroutine(CameraAttack(target));
        }

        caster.GetComponent<CharTurn>().ActionPoints -= card.APCost;
        caster.GetComponent<NewDeckDrawing>().StartCoroutine(caster.GetComponent<NewDeckDrawing>().MoveToDiscardPile(this.gameObject));
        canSelectTarget = false;
        selected = false;
        canHover = false;
        onHover = false;
        Arrow = false;
        


        for(int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++){
            caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().disableHovering = false;
            caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().canSelect = true;
        }
        
    }

    public void CardInUse(bool inUse){
        TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();
        for(int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++){
            if(caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().index != index){
                if(inUse){
                    caster.GetComponent<NewDeckDrawing>().hand[i].transform.DOMoveY(caster.GetComponent<NewDeckDrawing>().hand[i].transform.position.y - 5000, 0.5f);
                    caster.GetComponent<CharTurn>().turnUI.SetActive(false);
                    
                }

                if(!inUse){
                    caster.GetComponent<NewDeckDrawing>().hand[i].transform.DOMoveY(caster.GetComponent<NewDeckDrawing>().hand[i].transform.position.y + 5000, 0.5f);
                    if(tbm.isBattleFinished() == false){
                        CheckCurrentAPAgainstCard(caster.GetComponent<NewDeckDrawing>().hand[i]);
                        caster.GetComponent<CharTurn>().turnUI.SetActive(true);
                    }
                    
                }
            }
        }
    }

    public void AssingCaster(){
        Transform CardsParent = this.transform.parent;
        caster = CardsParent.parent.gameObject;
    }

    void Update(){

        if(canSelectTarget){
            SelectTarget();
        }
        
        if(Input.GetMouseButtonDown(0) && onHover && !selected && canSelect && !selected){
            if(caster.GetComponent<CharTurn>().ActionPoints >= card.APCost){

            if(card.attackMethod == CardTemplate.AttackMethod.Drag){
                if(caster.GetComponent<CharTurn>().ActionPoints >= card.APCost){
                    Drag = true;

                    //Disables cards being selected if a card in players hand is selected
                    for(int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++){
                        caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().canSelect = false;
                        caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().disableHovering = true;
                        //Debug.Log(caster.GetComponent<DeckDrawing>().hand[i].name + " " + caster.GetComponent<DeckDrawing>().hand[i].GetComponent<Card>().canSelect);
                    }
                }
                else{
                    StartCoroutine(cantUseCard());
                }
            }

            if(card.attackMethod == CardTemplate.AttackMethod.Arrow){
                if(caster.GetComponent<CharTurn>().ActionPoints >= card.APCost){
                    Arrow = true;
                    //Disables cards being selected if a card in players hand is selected
                    for(int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++){
                        caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().canSelect = false;
                        caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().disableHovering = true;
                        //Debug.Log(caster.GetComponent<DeckDrawing>().hand[i].name + " " + caster.GetComponent<DeckDrawing>().hand[i].GetComponent<Card>().canSelect);
                    }
                }
                else{
                    StartCoroutine(cantUseCard());
                }
                
            }

            selected = true;
            Debug.Log("selected true");
            canSelectTarget = true;
            CollectTarget();
        }
        else{
            StartCoroutine(cantUseCard());
        }
    }
        
        if(Input.GetMouseButtonUp(0) && selected){

            if(card.attackMethod == CardTemplate.AttackMethod.Drag){
                selected = false;
                Debug.Log("selected false");
                Drag = false;
                CheckIfCardIsPlayed();

                for(int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++){
                    caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().canSelect = true;
                    caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().disableHovering = false;
                    //Debug.Log(caster.GetComponent<DeckDrawing>().hand[i].name + " " + caster.GetComponent<DeckDrawing>().hand[i].GetComponent<Card>().canSelect);
                    
                }
            }
        }

        if(Input.GetMouseButtonDown(1) && onHover){
            selected = false;
            Debug.Log("selected false");
            canSelectTarget = false;
            if(card.attackMethod == CardTemplate.AttackMethod.Drag){
                Drag = false;
                transform.DOMove(originalPosition, 0.5f);
            }

            if(card.attackMethod == CardTemplate.AttackMethod.Arrow){
                Arrow = false;
            }

            for(int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++){
                    caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().canSelect = true;
                    caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().disableHovering = false;
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

        GameObject noAPBubble = GameObject.FindGameObjectWithTag("NoAPBubble");

        if(noAPBubble == null){
            GameObject bubble = Instantiate(speechBubblePrefab, caster.transform.position, Quaternion.identity);
            bubble.transform.position = new Vector3(bubble.transform.position.x + 1, bubble.transform.position.y + 2, bubble.transform.position.z);
            Destroy(bubble, 5);
        }
    }

    public void CheckCurrentAPAgainstCard(GameObject cards){
        if(cards.GetComponent<Card>().card.APCost > caster.GetComponent<CharTurn>().ActionPoints){
            foreach(Transform children in cards.transform){
                if(children.name.Contains("Outline")){
                    Image ImageOutline = children.GetComponent<Image>();
                    ImageOutline.color = cantUseColor;
                }
            }

            cards.GetComponent<CardRender>().APCost.color = cantUseColor;
        }
        else if(cards.GetComponent<Card>().card.APCost <= caster.GetComponent<CharTurn>().ActionPoints){
            foreach(Transform children in cards.transform){
                if(children.name.Contains("Outline")){
                    Image ImageOutline = children.GetComponent<Image>();
                    ImageOutline.color = canUseColor;
                }
            }

            cards.GetComponent<CardRender>().APCost.color = Color.white;
        }

        
            // Debug.Log("Counter " + counter + " handCount " + caster.GetComponent<NewDeckDrawing>().hand.Count);
            // cards.GetComponent<CardRender>().APCost.color = cantUseColor;

            // if(counter >= caster.GetComponent<NewDeckDrawing>().hand.Count){
            //     Debug.Log("Outline Button");
            // }
        
    }

    void CheckIfCardIsPlayed(){
        float YPos = originalPosition.y + 500;

        if(transform.position.y > YPos){
            CheckAbility(this.gameObject);
            selected = false;
            Drag = false;
            onHover = false;
        }
        else{
            transform.DOMove(originalPosition, 0.5f);
            canSelectTarget = false;
            targets.Clear();
        }
    }

    void MoveCardByMouse(){
        transform.position = Input.mousePosition;
    }

    public void Hover(){
        if(!Drag && canHover && !Arrow && !disableHovering){

        NewDeckDrawing dd = caster.GetComponent<NewDeckDrawing>();
        onHover = true;

        cardRotation = transform.eulerAngles;
        transform.DOScale(new Vector3(1f,1f,1f), 0.5f);
        transform.DORotate(new Vector3(0,0,0), 0.5f);

        if(index == 0 || index == 4){
            transform.DOMoveY(transform.position.y + 100, 0.5f);
        }

        if(index == 1 || index == 3){
            transform.DOMoveY(transform.position.y + 90, 0.5f);
        }

        if(index == 2){
            transform.DOMoveY(transform.position.y + 55, 0.5f);
        }

        
        
        
        int LastIndex = dd.hand.Count - 1;

        // if(dd.hand[0].GetComponent<Card>().index == index){
        //     dd.hand[0].transform.DOMoveY(transform.position.y + 225, 0.5f);
        // }
        // if(dd.hand[LastIndex].GetComponent<Card>().index == index){
        //     dd.hand[LastIndex].transform.DOMoveY(transform.position.y + 225, 0.5f);
        // }

        for(int i = 0; i < dd.hand.Count; i++){

            CloseCards.Add(dd.hand[i]);
            if(dd.hand[i].GetComponent<Card>().index < index - 1 || dd.hand[i].GetComponent<Card>().index > index + 1){
                CloseCards.Remove(dd.hand[i]);
                FarCards.Add(dd.hand[i]);
                CloseCards.Remove(this.gameObject);
            }
        }

        
        for(int i = 0; i < CloseCards.Count; i++){
            float[] Closeoffset = {CloseCards[i].GetComponent<Card>().originalPosition.x + 120, CloseCards[i].GetComponent<Card>().originalPosition.x - 120};
            if(index < CloseCards[i].GetComponent<Card>().index){
                CloseCards[i].transform.DOMoveX(Closeoffset[0], 0.5f);
            }
            else if(index > CloseCards[i].GetComponent<Card>().index){
                CloseCards[i].transform.DOMoveX(Closeoffset[1], 0.5f);
            }
        }

        for(int i = 0; i < FarCards.Count; i++){
            float[] Faroffset = {FarCards[i].GetComponent<Card>().originalPosition.x + 100, FarCards[i].GetComponent<Card>().originalPosition.x - 100};
            if(index < FarCards[i].GetComponent<Card>().index){
                FarCards[i].transform.DOMoveX(Faroffset[0], 0.5f);
            }
            else if(index > FarCards[i].GetComponent<Card>().index){
                FarCards[i].transform.DOMoveX(Faroffset[1], 0.5f);
            }
        }

        }
    
    }

    public void NoHover(){
        if(!Drag && canHover && !Arrow && !disableHovering){
        onHover = false;
        transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.5f);
        transform.DOMove(originalPosition, 0.5f);
        transform.DORotate(cardRotation, 0.5f);
        CloseCards.Clear();
        FarCards.Clear();

        NewDeckDrawing dd = caster.GetComponent<NewDeckDrawing>();
        for(int i = 0; i < dd.hand.Count; i++){
            dd.hand[i].transform.DOMove(dd.hand[i].GetComponent<Card>().originalPosition, 0.5f);
        }

    }

    }


    void Pull(GameObject target, GameObject target2){
            Vector3 target1Pos = target.transform.position;
            target.transform.DOMoveX(target2.transform.position.x, 0.5f);
            target2.transform.DOMoveX(target1Pos.x, 0.5f);
    }

    void Push(GameObject target, GameObject target2){
            Vector3 target1Pos = target.transform.position;
            target.transform.DOMoveX(target2.transform.position.x, 0.5f);
            target2.transform.DOMoveX(target1Pos.x, 0.5f);
    }

    void Retreat(GameObject target, GameObject target2){
        Vector3 target1Pos = target.transform.position;
        target.transform.DOMoveX(target2.transform.position.x, 0.5f);
        target2.transform.DOMoveX(target1Pos.x, 0.5f);
    }

    void GiveGuard(GameObject target, int ammount){

        GameObject shield = Instantiate(shieldIconPrefab, transform.position, Quaternion.identity);

        caster.GetComponent<CharacterController>().GainGuard(ammount);
    }

    void Heal(GameObject target, int ammount){

            int healAmmount = ammount;
            target.GetComponent<CharacterController>().Heal(healAmmount);
    }

    public void DealDamage(GameObject target, int ammount){
            target.GetComponent<CharacterController>().TakeDamage(ammount, "NormalDamage");
    }

    void Ignite(GameObject target, int ammount, int stack){

            target.GetComponent<CharacterController>().igniteAmmount = ammount;
            target.GetComponent<CharacterController>().igniteStack += stack;
    }

    void Poison(GameObject target, int ammount, int stack){

            target.GetComponent<CharacterController>().poisonAmmount = ammount;
            target.GetComponent<CharacterController>().poisonStack += stack;
    }

    void Chilled(GameObject target, int stack){
        target.GetComponent<CharacterController>().chilledStack += stack;
    }

    void Invisible(GameObject target, int stack){
        target.GetComponent<CharacterController>().invisibleStack = stack;
        target.GetComponent<CharacterController>().Invisible();
    }

    void DealPartyDamage(int ammount){
            List<GameObject> targets = FindEnemies();

            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().TakeDamage(ammount, "NormalDamage");
            }
    }
   
    void IgniteAllEnemies(int ammount, int stack){
            List<GameObject> targets = FindEnemies();
            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().igniteAmmount = ammount;
                targets[i].GetComponent<CharacterController>().igniteStack += stack;
            }
    }

   void HealParty(int ammount){
            Debug.Log("Healing Party");
            List<GameObject> targets = FindGoodChar();

            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().Heal(ammount);
            }
   }

   public IEnumerator CameraAttack(GameObject targetPos){
        CardInUse(true);
        
        // if(!affectsCaster){

        Vector3 ogPos = caster.transform.position;
        Debug.Log(ogPos);
        Camera cam = Camera.main;
        cam.GetComponent<CameraZoom>().shouldZoomIn = true;
        caster.transform.DOMoveX(cam.GetComponent<CameraZoom>().target.position.x, 1);
        yield return new WaitForSeconds(1);
        if(card.AttackEffect != null){
            GameObject effect = Instantiate(card.AttackEffect, targetPos.transform.position, Quaternion.identity);
            Destroy(effect, 1.6f);
                
        }
        yield return new WaitForSeconds(2);

        cam.GetComponent<CameraZoom>().shouldZoomIn = false;
        caster.transform.DOMove(ogPos, 1);
        //}

        // if(affectsCaster){
        //     yield return new WaitForSeconds(3f);
        // }
            CardInUse(false);
        
            DestroyImmediate(this.gameObject, true);
        
   }


}
