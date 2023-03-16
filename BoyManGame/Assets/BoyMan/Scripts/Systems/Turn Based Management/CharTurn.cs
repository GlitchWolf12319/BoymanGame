using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharTurn : FindTargets
{
    public enum CharacterType {Player, Enemy}
    public CharacterType characterType;
    public EnemyTurn enemyTurn;
    public TurnBaseManager tbm;
    public int TurnCounter;
    public int abilityTurnCounter;


    void Start(){
        if(characterType == CharacterType.Player){
            enemyTurn = null;
        }
    }

    public IEnumerator StartTurn(){
        TurnCounter++;
        transform.GetComponent<CharacterController>().CheckStatusEffects();
        if(characterType == CharacterType.Enemy){
            enemyTurn.SetTarget(TurnCounter);
            if(TurnCounter > enemyTurn.TurnMoves[TurnCounter].cards.Length){
                TurnCounter = 0;
            }
            yield return new WaitForSeconds(enemyTurn.thinkTime);
            StartCoroutine(GetTarget());
        }

        if(characterType == CharacterType.Player){
                
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
            EndTurn();
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

    void Retreat(GameObject target1, GameObject target2, int index){}

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

    public void GainCardInfo(CardTemplate card, GameObject target, int stack, int ammount, int index){
        if(characterType == CharacterType.Player){

            if(card.ability[index].dealDamage != null){
                DealDamage(target, index, ammount);
            }

            if(card.ability[index].igniteEffect != null){
                 Ignite(target, index, ammount, stack);   
            }

            if(card.ability[index].poisonEffect != null){
                 Poison(target, index, ammount, stack);   
            }

            if(card.ability[index].guard != null){
                 GiveGuard(target, ammount, index);  
            }

            if(card.ability[index].heal != null){
                 Heal(target, ammount, index);  
            }

            if(card.ability[index].chilled != null){
                 Chilled(target, stack, index);  
            }
        }
    }



    void EndTurn(){
        if(tbm != null){
            abilityTurnCounter = 0;
            tbm.ChangeTurn();
        }
    }
}
