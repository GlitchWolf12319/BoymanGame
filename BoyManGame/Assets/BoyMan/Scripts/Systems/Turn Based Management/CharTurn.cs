using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

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
        this.GetComponent<CharacterController>().CheckStatusEffects();
        if(characterType == CharacterType.Enemy){

            if(TurnCounter > enemyTurn.TurnMoves.Length - 1){
                TurnCounter = 0;
            }
            enemyTurn.SetTarget(TurnCounter);
            yield return new WaitForSeconds(enemyTurn.thinkTime);
            StartCoroutine(GetTarget());
        }

        if(characterType == CharacterType.Player){
            ActionPoints = this.GetComponent<CharacterController>().CS.MaxAP;
            deck.SetActive(true);
            transform.GetComponent<DeckDrawing>().DrawCards(5);
        }
    }

    IEnumerator GetTarget(){
        if(characterType == CharacterType.Enemy){

            for(int i = 0; i < enemyTurn.TurnMoves[TurnCounter].cards.Length; i++){
                if(enemyTurn.target == EnemyTurn.Target.Player && enemyTurn.TurnMoves[TurnCounter].cards[i].dealDamage != null){
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
            
                if(enemyTurn.target == EnemyTurn.Target.Player && enemyTurn.TurnMoves[TurnCounter].cards[i].pull != null){
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
            
                if(enemyTurn.target == EnemyTurn.Target.Enemy && enemyTurn.TurnMoves[TurnCounter].cards[i].retreat != null){
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
        
                if(enemyTurn.target == EnemyTurn.Target.Enemy && enemyTurn.TurnMoves[TurnCounter].cards[i].guard != null){
                    GiveGuard(this.gameObject, 0, i);
                    abilityTurnCounter++;
                    CheckToEndTurn();
                }
        
                if(enemyTurn.target == EnemyTurn.Target.Enemy && enemyTurn.TurnMoves[TurnCounter].cards[i].heal != null){
                    GiveGuard(this.gameObject, 0, i);
                    abilityTurnCounter++;
                    CheckToEndTurn();
                }

        }
        }
    }

    // IEnumerator UseAbility(GameObject target, GameObject target2, int index){
    //     if(characterType == CharacterType.Enemy){

    //         if(enemyTurn.TurnMoves[TurnCounter].cards[index] != null && enemyTurn.TurnMoves[TurnCounter].cards[index].dealDamage != null){
    //             target.GetComponent<CharacterController>().TakeDamage(enemyTurn.TurnMoves[TurnCounter].cards[index].dealDamage.damageAmmount);
    //         }

    //         yield return new WaitForSeconds(1);

    //         if(enemyTurn.TurnMoves[TurnCounter].cards[index] != null && enemyTurn.TurnMoves[TurnCounter].cards[index].poisonEffect != null){
    //             target.GetComponent<CharacterController>().poisonAmmount = enemyTurn.TurnMoves[TurnCounter].cards[index].poisonEffect.poisonAmmount;
    //             target.GetComponent<CharacterController>().poisonStack += enemyTurn.TurnMoves[TurnCounter].cards[index].poisonEffect.poisonStack;
    //         }

    //         yield return new WaitForSeconds(1);

    //         if(enemyTurn.TurnMoves[TurnCounter].cards[index] != null && enemyTurn.TurnMoves[TurnCounter].cards[index].igniteEffect != null){
    //             target.GetComponent<CharacterController>().igniteAmmount = enemyTurn.TurnMoves[TurnCounter].cards[index].igniteEffect.IgniteAmmount;
    //             target.GetComponent<CharacterController>().igniteStack += enemyTurn.TurnMoves[TurnCounter].cards[index].igniteEffect.IgniteStack;
    //         }

    //         yield return new WaitForSeconds(1);

    //         if(enemyTurn.TurnMoves[TurnCounter].cards[index] != null && enemyTurn.TurnMoves[TurnCounter].cards[index].pull != null || enemyTurn.TurnMoves[TurnCounter].cards[index].push != null){
    //             Vector3 target1Pos = target.transform.position;
    //             target.transform.DOMoveX(target2.transform.position.x, 0.5f);
    //             target2.transform.DOMoveX(target1Pos.x, 0.5f);
    //         }

    //         yield return new WaitForSeconds(1);

    //         if(enemyTurn.TurnMoves[TurnCounter].cards[index] != null && enemyTurn.TurnMoves[TurnCounter].cards[index].retreat != null){
    //             Vector3 target1Pos = target.transform.position;
    //             target.transform.DOMoveX(target2.transform.position.x, 0.5f);
    //             target2.transform.DOMoveX(target1Pos.x, 0.5f);
    //         }

    //         yield return new WaitForSeconds(enemyTurn.thinkTime);
    //         if(abilityTurnCounter == enemyTurn.TurnMoves[TurnCounter].cards.Length){
    //             EndTurn();
    //         }
    //     }
    // }

    void CheckToEndTurn(){
        if(abilityTurnCounter == enemyTurn.TurnMoves[TurnCounter].cards.Length){
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
        if(characterType == CharacterType.Enemy){
            int guardAmmount = enemyTurn.TurnMoves[TurnCounter].cards[index].guard.GuardValue();
            target.GetComponent<CharacterController>().GainGuard(guardAmmount);
        }
        
        if(characterType == CharacterType.Player){
            target.GetComponent<CharacterController>().GainGuard(ammount);
        }
    }

    void Heal(GameObject target, int ammount, int index){
        if(characterType == CharacterType.Enemy){
            int healAmmount = enemyTurn.TurnMoves[TurnCounter].cards[index].heal.GetHealValue();
            target.GetComponent<CharacterController>().Heal(healAmmount);
        }

        if(characterType == CharacterType.Player){
            int healAmmount = ammount;
            target.GetComponent<CharacterController>().Heal(healAmmount);
        }
    }

    public void DealDamage(GameObject target, int index, int ammount){
        if(characterType == CharacterType.Enemy){
            int damageAmmount = enemyTurn.TurnMoves[TurnCounter].cards[index].dealDamage.GetDamageValue();
            target.GetComponent<CharacterController>().TakeDamage(damageAmmount);
        }

        if(characterType == CharacterType.Player){
            int damageAmmount = ammount;
            target.GetComponent<CharacterController>().TakeDamage(damageAmmount);
        }
        
    }

    void Ignite(GameObject target, int index, int ammount, int stack){
        if(characterType == CharacterType.Enemy){
            target.GetComponent<CharacterController>().igniteAmmount = enemyTurn.TurnMoves[TurnCounter].cards[index].igniteEffect.IgniteAmmount;
            target.GetComponent<CharacterController>().igniteStack += enemyTurn.TurnMoves[TurnCounter].cards[index].igniteEffect.IgniteStack;
        }

        if(characterType == CharacterType.Player){
            target.GetComponent<CharacterController>().igniteAmmount = ammount;
            target.GetComponent<CharacterController>().igniteStack += stack;
        }
    }

    void Poison(GameObject target, int index, int ammount, int stack){
        if(characterType == CharacterType.Enemy){
                target.GetComponent<CharacterController>().poisonAmmount = enemyTurn.TurnMoves[TurnCounter].cards[index].poisonEffect.poisonAmmount;
                target.GetComponent<CharacterController>().poisonStack += enemyTurn.TurnMoves[TurnCounter].cards[index].poisonEffect.poisonStack;
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
                targets[i].GetComponent<CharacterController>().TakeDamage(ammount);
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
   
    public void GainCardInfo(CardTemplate cardTemp, Card card, GameObject target, int stack, int ammount, int index, int APCost){
        if(characterType == CharacterType.Player){

            if(ActionPoints >= APCost){
                ActionPoints -= APCost;
                card.canSelectTarget = false;
                card.selected = false;
                card.targets.Clear();
                this.GetComponent<DeckDrawing>().StartCoroutine(this.GetComponent<DeckDrawing>().MoveToDiscardPile(card));

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
        StartCoroutine(EndTurn());
    }
    public IEnumerator EndTurn(){
        if(tbm != null){
            abilityTurnCounter = 0;
            yield return new WaitForSeconds(1);
            tbm.ChangeTurn();
            yield return new WaitForSeconds(1);
            if(characterType == CharacterType.Player){
                deck.SetActive(false);
            }
            
        }
    }
}
