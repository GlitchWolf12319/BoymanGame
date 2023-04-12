using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class CharTurn : EnemyAbility
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
    public GameObject intentionIcon;
    public TMP_Text damageAmmountText;
    public Sprite swordIcon;
    public Sprite blockIcon;
    public Sprite igniteIcon;

    void Start(){
        
        tbm = FindObjectOfType<TurnBaseManager>();
        if(characterType == CharacterType.Player){
            enemyTurn = null;
            ActionPoints = this.GetComponent<CharacterController>().CS.startingAP;
        }

        if(characterType == CharacterType.Enemy){
            IntentSystem();
        }
    }

    void Update(){
        if(characterType == CharacterType.Player){
            APText.text = ActionPoints + "/" + transform.GetComponent<CharacterController>().CS.MaxAP.ToString();
        }
    }

    public IEnumerator StartTurn(){
        if(!transform.GetComponent<CharacterController>().dead){

        TurnCounter++;
        turnMoveCounter++;

        turnBanner = GameObject.FindGameObjectWithTag("TurnBanner");

        this.GetComponent<CharacterController>().CheckStatusEffects();

        if(characterType == CharacterType.Enemy){

            //IntentSystem();

            turnBanner.GetComponent<TurnBanner>().currentTurn = TurnBanner.Turn.Enemy;

            turnBanner.GetComponent<TurnBanner>().turnText.text = "Enemies Turn";
            int displayTurn = TurnCounter + 1;
            turnBanner.GetComponent<TurnBanner>().turnCounterText.text = "Turn " + displayTurn;
            
            if(turnMoveCounter > enemyTurn.TurnMoves.Length - 1){
                turnMoveCounter = 0;
            }
            enemyTurn.SetTarget(turnMoveCounter);
            turnBanner.GetComponent<Animator>().Play("PlayerTurn");

            yield return new WaitForSeconds(enemyTurn.thinkTime);


            //checks if targets are there to be attacked
            List<GameObject> anyTargets = FindGoodChar();
            int counter = 0;
            for(int i = 0; i < anyTargets.Count; i++){
                if(anyTargets[i].GetComponent<CharacterController>().dead){
                    counter++;
                }
            }

            if(counter < anyTargets.Count && !transform.GetComponent<CharacterController>().dead){
                StartCoroutine(GetTarget());
            }
            
        }

        if(characterType == CharacterType.Player){
            turnBanner.GetComponent<TurnBanner>().currentTurn = TurnBanner.Turn.Player;

            if(transform.name.Contains("BoyMan")){
                turnBanner.GetComponent<TurnBanner>().turnText.text = "BoyMan's Turn";
            }

            if(transform.name.Contains("Jane")){
                turnBanner.GetComponent<TurnBanner>().turnText.text = "Jane's Turn";
            }
            
            int displayTurn = TurnCounter + 1;
            turnBanner.GetComponent<TurnBanner>().turnCounterText.text = "Turn " + displayTurn;
            turnBanner.GetComponent<Animator>().Play("PlayerTurn");
            ActionPoints = this.GetComponent<CharacterController>().CS.MaxAP;
            yield return new WaitForSeconds(1);
            deck.SetActive(true);
            turnUI.SetActive(true);
            transform.GetComponent<NewDeckDrawing>().DrawCards(5);
        }
    }
    }

    public void IntentSystem(){
        int intentCounter = turnMoveCounter + 1;
        Debug.Log(intentCounter);

        if(intentCounter > enemyTurn.TurnMoves.Length - 1){
            intentCounter = 0;
        }


        if(enemyTurn.TurnMoves[intentCounter].cards[0].dealDamage != null){
            Color fullAlpha = new Color(fullAlpha.r = 255, fullAlpha.g = 255, fullAlpha.b = 255, fullAlpha.a = 255);
            damageAmmountText.text = enemyTurn.TurnMoves[intentCounter].cards[0].dealDamage.damageAmmount.ToString();
            intentionIcon.GetComponent<Image>().color = fullAlpha;
            intentionIcon.GetComponent<Image>().sprite = swordIcon;
        }

        if(enemyTurn.TurnMoves[intentCounter].cards[0].guard != null){
            Color fullAlpha = new Color(fullAlpha.r = 255, fullAlpha.g = 255, fullAlpha.b = 255, fullAlpha.a = 255);
            damageAmmountText.text = enemyTurn.TurnMoves[intentCounter].cards[0].guard.guardAmmount.ToString();
            intentionIcon.GetComponent<Image>().color = fullAlpha;
            intentionIcon.GetComponent<Image>().sprite = blockIcon;
        }

        if(enemyTurn.TurnMoves[intentCounter].cards[0].igniteEffect != null){
            Color fullAlpha = new Color(fullAlpha.r = 255, fullAlpha.g = 255, fullAlpha.b = 255, fullAlpha.a = 255);
            damageAmmountText.text = enemyTurn.TurnMoves[intentCounter].cards[0].igniteEffect.IgniteStack.ToString();
            intentionIcon.GetComponent<Image>().color = fullAlpha;
            intentionIcon.GetComponent<Image>().sprite = igniteIcon;
        }
    }

    IEnumerator GetTarget(){
        if(characterType == CharacterType.Enemy){

            for(int i = 0; i < enemyTurn.TurnMoves[turnMoveCounter].cards.Length; i++){
                if(enemyTurn.target == EnemyTurn.Target.Player && enemyTurn.TurnMoves[turnMoveCounter].cards[i].dealDamage != null){
                    List<GameObject> targets = FindGoodChar();

                    if(targets != null){
                        Debug.Log("Choosing Target");
                        int randomChoice = Random.Range(0, targets.Count);
                        GameObject target = targets[randomChoice];
                        yield return new WaitForSeconds(1);
                        Debug.Log("Dealing Damage");
                        DealDamage(target, enemyTurn.TurnMoves[turnMoveCounter].cards[i].dealDamage.damageAmmount);
                        StartCoroutine(EnemyCameraAttack());
                        abilityTurnCounter++;
                        CheckToEndTurn();
                        targets.Clear();
                        target = null;
                    }
                }

                if(enemyTurn.target == EnemyTurn.Target.Player && enemyTurn.TurnMoves[turnMoveCounter].cards[i].igniteEffect != null){
                    List<GameObject> targets = FindGoodChar();

                    if(targets != null){
                        Debug.Log("Choosing Target");
                        int randomChoice = Random.Range(0, targets.Count);
                        GameObject target = targets[randomChoice];
                        yield return new WaitForSeconds(1);
                        Debug.Log("Dealing Ignite Damage");
                        Ignite(target, enemyTurn.TurnMoves[turnMoveCounter].cards[i].igniteEffect.IgniteAmmount, enemyTurn.TurnMoves[turnMoveCounter].cards[i].igniteEffect.IgniteStack);
                        StartCoroutine(EnemyCameraAttack());
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
                        Retreat(this.gameObject, RetreatTarget);
                        abilityTurnCounter++;
                        CheckToEndTurn();
                        targets.Clear();
                    }
                    
                }
            }
        
                if(enemyTurn.target == EnemyTurn.Target.Enemy && enemyTurn.TurnMoves[turnMoveCounter].cards[i].guard != null){
                    GiveGuard(this.gameObject, enemyTurn.TurnMoves[turnMoveCounter].cards[i].guard.guardAmmount , shieldIconPrefab);
                    abilityTurnCounter++;
                    CheckToEndTurn();
                }
        
                if(enemyTurn.target == EnemyTurn.Target.Enemy && enemyTurn.TurnMoves[turnMoveCounter].cards[i].heal != null){
                    Heal(this.gameObject, enemyTurn.TurnMoves[turnMoveCounter].cards[i].heal.healAmmount);
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


   public IEnumerator EnemyCameraAttack(){
        Vector3 ogPos = transform.position;
        Camera cam = Camera.main;
        cam.GetComponent<CameraZoom>().shouldZoomIn = true;
        transform.DOMoveX(cam.GetComponent<CameraZoom>().target.position.x, 1);
        yield return new WaitForSeconds(2);
        cam.GetComponent<CameraZoom>().shouldZoomIn = false;
        transform.DOMove(ogPos, 1);
   }

    public void EndTurnButton(){
        int counter = 0;
        for(int i = 0; i < transform.GetComponent<NewDeckDrawing>().hand.Count; i++){
            if(transform.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().selected == true){
                counter++;
            }
        }

        if(counter == 0){
            Debug.Log("canSkip");
            StartCoroutine(EndTurn());
        }
        
    }


    public IEnumerator EndTurn(){
        yield return new WaitForSeconds(2);
        if(tbm != null){
            if(characterType == CharacterType.Player){
                turnUI.SetActive(false);
            }

            if(characterType == CharacterType.Enemy){
                IntentSystem();
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
