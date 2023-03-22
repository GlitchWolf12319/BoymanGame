using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class CharTurn : FindTargets
{
    public enum CharacterType {Player, Enemy}
    public CharacterType characterType;
    public EnemyTurn enemyTurn;
    public TurnBaseManager tbm;
    public int TurnCounter;
    public int abilityTurnCounter;
    public GameObject deck;
    public int ActionPoints;
    public TMP_Text APText;
    public GameObject turnBanner;
    private int cardCompleteCounter;
    public GameObject shieldIconPrefab;
    public GameObject attackIconPrefab;
    public int turnMoveCounter;
    public GameObject turnUI;

    void Start(){
        
        tbm = FindObjectOfType<TurnBaseManager>();
        if(characterType == CharacterType.Player){
            enemyTurn = null;
            ActionPoints = this.GetComponent<CharacterController>().CS.startingAP;
        }
    }

    void Update(){
        if(characterType == CharacterType.Player){
            APText.text = ActionPoints + "/" + transform.GetComponent<CharacterController>().CS.MaxAP.ToString();
        }
        
    }

    public IEnumerator StartTurn(){
        TurnCounter++;
        turnMoveCounter++;

        turnBanner = GameObject.FindGameObjectWithTag("TurnBanner");


        this.GetComponent<CharacterController>().CheckStatusEffects();

        if(characterType == CharacterType.Enemy){

            turnBanner.GetComponent<TurnBanner>().currentTurn = TurnBanner.Turn.Enemy;
            turnBanner.GetComponent<TurnBanner>().turnText.text = "Enemies Turn";
            int displayTurn = TurnCounter + 1;
            turnBanner.GetComponent<TurnBanner>().turnCounterText.text = "Turn " + displayTurn;
            
            if(turnMoveCounter > enemyTurn.TurnMoves.Length - 1){
                turnMoveCounter = 0;
            }
            enemyTurn.SetTarget(turnMoveCounter);
            yield return new WaitForSeconds(enemyTurn.thinkTime);

            List<GameObject> anyTargets = FindGoodChar();
            int counter = 0;
            for(int i = 0; i < anyTargets.Count; i++){
                if(anyTargets[i].GetComponent<CharacterController>().dead){
                    counter++;
                }
            }

            if(counter < anyTargets.Count){
                StartCoroutine(GetTarget());
            }
            
        }

        if(characterType == CharacterType.Player){
            turnBanner.GetComponent<TurnBanner>().currentTurn = TurnBanner.Turn.Player;
            turnBanner.GetComponent<TurnBanner>().turnText.text = "Player's Turn";
            int displayTurn = TurnCounter + 1;
            turnBanner.GetComponent<TurnBanner>().turnCounterText.text = "Turn " + displayTurn;
            ActionPoints = this.GetComponent<CharacterController>().CS.MaxAP;
            yield return new WaitForSeconds(1);
            deck.SetActive(true);
            turnUI.SetActive(true);
            transform.GetComponent<DeckDrawing>().DrawCards(5);
        }

        turnBanner.GetComponent<Animator>().Play("PlayerTurn");
    }

    IEnumerator GetTarget(){
        if(characterType == CharacterType.Enemy){

            for(int i = 0; i < enemyTurn.TurnMoves[turnMoveCounter].cards.Length; i++){
                if(enemyTurn.target == EnemyTurn.Target.Player && enemyTurn.TurnMoves[turnMoveCounter].cards[i].dealDamage != null){
                    List<GameObject> targets = FindGoodChar();

                    if(targets != null){
                        int randomChoice = Random.Range(0, targets.Count);
                        GameObject target = targets[randomChoice];
                        yield return new WaitForSeconds(1);
                        DealDamage(target, i, 0);
                        abilityTurnCounter++;
                        CheckToEndTurn();
                        targets.Clear();
                        target = null;
                    }
                }
            
                if(enemyTurn.target == EnemyTurn.Target.Player && enemyTurn.TurnMoves[turnMoveCounter].cards[i].pull != null){
                List<GameObject> targets = FindGoodChar();
                if(targets != null){
                    List<GameObject> pullTargets = new List<GameObject>();
                    for(int j = 0; j < 2; j++){
                        int randomChoice = Random.Range(0, targets.Count);
                        pullTargets.Add(targets[randomChoice]);
                        targets.Remove(targets[randomChoice]);
                    }
                    yield return new WaitForSeconds(1);
                    Pull(pullTargets[0], pullTargets[1]);
                    abilityTurnCounter++;
                    CheckToEndTurn();
                    pullTargets.Clear();
                    targets.Clear(); 
                }
            }
            
                if(enemyTurn.target == EnemyTurn.Target.Enemy && enemyTurn.TurnMoves[turnMoveCounter].cards[i].retreat != null){
                List<GameObject> targets = FindEnemies();
                if(targets != null){
                    GameObject RetreatTarget = null;
                    while(RetreatTarget == null){
                        int randomChoice = Random.Range(0, targets.Count);
                        if(targets[randomChoice].transform.name != this.transform.name){
                            RetreatTarget = targets[randomChoice];
                        }
                    }
                    if(RetreatTarget != null){
                        yield return new WaitForSeconds(1);
                        Retreat(this.gameObject, RetreatTarget, i);
                        abilityTurnCounter++;
                        CheckToEndTurn();
                        targets.Clear();
                    }
                    
                }
            }
        
                if(enemyTurn.target == EnemyTurn.Target.Enemy && enemyTurn.TurnMoves[turnMoveCounter].cards[i].guard != null){
                    GiveGuard(this.gameObject, 0, i);
                    abilityTurnCounter++;
                    CheckToEndTurn();
                }
        
                if(enemyTurn.target == EnemyTurn.Target.Enemy && enemyTurn.TurnMoves[turnMoveCounter].cards[i].heal != null){
                    GiveGuard(this.gameObject, 0, i);
                    abilityTurnCounter++;
                    CheckToEndTurn();
                }

        }
        }
    }

    void CheckToEndTurn(){
        if(abilityTurnCounter == enemyTurn.TurnMoves[turnMoveCounter].cards.Length){
            StartCoroutine(EndTurn());

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

    void Retreat(GameObject target, GameObject target2, int index){
        Vector3 target1Pos = target.transform.position;
        target.transform.DOMoveX(target2.transform.position.x, 0.5f);
        target2.transform.DOMoveX(target1Pos.x, 0.5f);
    }

    void GiveGuard(GameObject target, int ammount, int index){

        GameObject shield = Instantiate(shieldIconPrefab, transform.position, Quaternion.identity);

        if(characterType == CharacterType.Enemy){
            int guardAmmount = enemyTurn.TurnMoves[turnMoveCounter].cards[index].guard.GuardValue();
            target.GetComponent<CharacterController>().GainGuard(guardAmmount);
        }
        
        if(characterType == CharacterType.Player){
            target.GetComponent<CharacterController>().GainGuard(ammount);
        }
    }

    void Heal(GameObject target, int ammount, int index){
        if(characterType == CharacterType.Enemy){
            int healAmmount = enemyTurn.TurnMoves[turnMoveCounter].cards[index].heal.GetHealValue();
            target.GetComponent<CharacterController>().Heal(healAmmount);
        }

        if(characterType == CharacterType.Player){
            int healAmmount = ammount;
            target.GetComponent<CharacterController>().Heal(healAmmount);
        }
    }

    public void DealDamage(GameObject target, int index, int ammount){
        GameObject sword = Instantiate(attackIconPrefab, transform.position, Quaternion.identity);

        if(characterType == CharacterType.Enemy){
            int damageAmmount = enemyTurn.TurnMoves[turnMoveCounter].cards[index].dealDamage.GetDamageValue();
            if(target != null){
                target.GetComponent<CharacterController>().TakeDamage(damageAmmount, "Damage");
            }
            
        }

        if(characterType == CharacterType.Player){
            int damageAmmount = ammount;
            target.GetComponent<CharacterController>().TakeDamage(damageAmmount, "Damage");
        }
        
    }

    void Ignite(GameObject target, int index, int ammount, int stack){
        if(characterType == CharacterType.Enemy){
            target.GetComponent<CharacterController>().igniteAmmount = enemyTurn.TurnMoves[turnMoveCounter].cards[index].igniteEffect.IgniteAmmount;
            target.GetComponent<CharacterController>().igniteStack += enemyTurn.TurnMoves[turnMoveCounter].cards[index].igniteEffect.IgniteStack;
        }

        if(characterType == CharacterType.Player){
            target.GetComponent<CharacterController>().igniteAmmount = ammount;
            target.GetComponent<CharacterController>().igniteStack += stack;
        }
    }

    void Poison(GameObject target, int index, int ammount, int stack){
        if(characterType == CharacterType.Enemy){
                target.GetComponent<CharacterController>().poisonAmmount = enemyTurn.TurnMoves[turnMoveCounter].cards[index].poisonEffect.poisonAmmount;
                target.GetComponent<CharacterController>().poisonStack += enemyTurn.TurnMoves[turnMoveCounter].cards[index].poisonEffect.poisonStack;
        }

        if(characterType == CharacterType.Player){
            target.GetComponent<CharacterController>().poisonAmmount = ammount;
            target.GetComponent<CharacterController>().poisonStack += stack;
        }
    }

    void Chilled(GameObject target, int stack, int index){
        target.GetComponent<CharacterController>().chilledStack += stack;
    }

    void Invisible(GameObject target, int stack, int index){
        target.GetComponent<CharacterController>().invisibleStack = stack;
        target.GetComponent<CharacterController>().Invisible();
    }

    void DealPartyDamage(int ammount){
        if(characterType == CharacterType.Player){
            List<GameObject> targets = FindEnemies();

            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().TakeDamage(ammount, "Damage");
            }
        }
    }
   
    void IgniteAllEnemies(int ammount, int stack){
        if(characterType == CharacterType.Player){
            List<GameObject> targets = FindEnemies();
            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().igniteAmmount = ammount;
                targets[i].GetComponent<CharacterController>().igniteStack += stack;
            }
            
        }
    }
   void HealParty(int ammount){
        if(characterType == CharacterType.Player){
            Debug.Log("Healing Party");
            List<GameObject> targets = FindGoodChar();

            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().Heal(ammount);
            }
        }
   }
   
   public IEnumerator CameraAttack(CardTemplate cardTemp, GameObject targetPos){
        Debug.Log("test");
        Vector3 ogPos = transform.position;
        Camera cam = Camera.main;
        cam.GetComponent<CameraZoom>().shouldZoomIn = true;
        transform.DOMove(cam.GetComponent<CameraZoom>().target.position, 1);
        yield return new WaitForSeconds(1);
        if(cardTemp.AttackEffect != null){
                Debug.Log("test");
                GameObject effect = Instantiate(cardTemp.AttackEffect, targetPos.transform.position, Quaternion.identity);
                Destroy(effect, 1.6f);
                
            }

        yield return new WaitForSeconds(2);
        cam.GetComponent<CameraZoom>().shouldZoomIn = false;
        transform.DOMove(ogPos, 1);
   }

   public IEnumerator CardAnimation(GameObject card){
        Vector3 middle = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        card.transform.DOMove(middle, 1);
        yield return new WaitForSeconds(0.5f);
        card.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f);
        //card.GetComponent<Card>().trail.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        card.transform.DORotate(new Vector3(0, 0, -145), 0.5f);
        yield return new WaitForSeconds(0.5f);
        //card.transform.DOMove(new Vector3(card.transform.position.x + 2000, card.transform.position.y - 2000, card.transform.position.z), 1);
   
        card.transform.DOMove(new Vector3(card.transform.position.x + 2000, card.transform.position.y - 2000, card.transform.position.z), 1).OnComplete(() =>
        {
            card.transform.DOScale(new Vector3(1.03905f, 1.03905f, 1.03905f), 0.5f);
            card.transform.eulerAngles = new Vector3(0,0,0);
            card.transform.position = new Vector3(card.GetComponent<Card>().originalPosition.x - 1600, card.GetComponent<Card>().originalPosition.y, card.GetComponent<Card>().originalPosition.z);
        });
   
   }

    public void GainCardInfo(CardTemplate cardTemp, Card card, GameObject target, int stack, int ammount, int index, int APCost){
        if(characterType == CharacterType.Player){

            if(ActionPoints >= APCost){


                cardCompleteCounter++;
                if(cardCompleteCounter == cardTemp.ability.Length){
                    ActionPoints -= APCost;
                    StartCoroutine(CardAnimation(card.gameObject));
                    this.GetComponent<DeckDrawing>().StartCoroutine(this.GetComponent<DeckDrawing>().MoveToDiscardPile(card));
                    card.canSelectTarget = false;
                    card.selected = false;
                    card.canHover = false;
                    card.onHover = false;
                    card.Arrow = false;
                    if(target.tag != "Player"){
                        StartCoroutine(CameraAttack(cardTemp, target));
                    }
                    card.targets.Clear();
                    cardCompleteCounter = 0;

                    for(int i = 0; i < this.GetComponent<DeckDrawing>().hand.Count; i++){
                        this.GetComponent<DeckDrawing>().hand[i].GetComponent<Card>().CheckCurrentAPAgainstCard();
                    }
                }

            if(cardTemp.ability[index].dealDamage != null){
                DealDamage(target, index, ammount);
            }

            if(cardTemp.ability[index].igniteEffect != null){
                 Ignite(target, index, ammount, stack);   
            }

            if(cardTemp.ability[index].poisonEffect != null){
                 Poison(target, index, ammount, stack);   
            }

            if(cardTemp.ability[index].guard != null){
                 GiveGuard(target, ammount, index);  
            }

            if(cardTemp.ability[index].heal != null){
                 Heal(target, ammount, index);  
            }

            if(cardTemp.ability[index].chilled != null){
                 Chilled(target, stack, index);  
            }

            if(cardTemp.ability[index].invisible != null){
                 Invisible(target, stack, index);  
            }

            if(cardTemp.ability[index].retreat != null){
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
                        Retreat(target, RetreatTarget, 0);
                        possibleTargets.Clear();
                     }
                }
            }
        
            if(cardTemp.ability[index].dealPartyDamage != null){
                DealPartyDamage(ammount);
            }
        
            if(cardTemp.ability[index].healParty != null){
                HealParty(ammount);
            }
        
            if(cardTemp.ability[index].igniteParty != null){
                IgniteAllEnemies(ammount, stack);
            }
        
            
        
        
        }

        }
    }


    public void EndTurnButton(){
        int counter = 0;
        for(int i = 0; i < transform.GetComponent<DeckDrawing>().hand.Count; i++){
            if(transform.GetComponent<DeckDrawing>().hand[i].GetComponent<Card>().selected == true){
                counter++;
            }
        }

        if(counter == 0){
            Debug.Log("canSkip");
            StartCoroutine(EndTurn());
        }
        
    }


    public IEnumerator EndTurn(){
        if(tbm != null){
            if(characterType == CharacterType.Player){
                turnUI.SetActive(false);
            }
            abilityTurnCounter = 0;
            yield return new WaitForSeconds(1.5f);
            tbm.ChangeTurn();
            yield return new WaitForSeconds(1.5f);
            if(characterType == CharacterType.Player){
                deck.SetActive(false);
            }

        }
    }
}
