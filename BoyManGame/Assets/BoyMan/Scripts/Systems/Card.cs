using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] private CardTemplate card;
    [SerializeField]private bool selected;
    public Vector3 originalPosition;
    public int index;

    private List<Card> CloseCards = new List<Card>();
    private List<Card> FarCards = new List<Card>();
    private bool canSelectTarget;
    [SerializeField]private List<GameObject> targets = new List<GameObject>();
    
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
    }

    public void CollectTarget(){
        targets = card.ability.ReturnTargets();
    }


    public void SelectTarget(){
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Perform the raycast and get the hit information
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100);

            // Check if the raycast hit a 2D GameObject
            if (hit.collider != null && hit.collider.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                // Add the GameObject to the list of selected objects
                for(int i = 0; i < targets.Count; i++){
                    if(hit.collider.gameObject.tag == targets[i].tag){
                        CheckAbility(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    void CheckAbility(GameObject target){
        DeckDrawing dd = FindObjectOfType<DeckDrawing>();
        canSelectTarget = false;
        targets.Clear();
        dd.MoveToDiscardPile(this);

        if(card.ability.cardAbility == Ability.CardAbility.Damage){
            target.GetComponent<EnemyController>().TakeDamage(card.ability.damageAmmount);
        }

        if(card.ability.cardAbility == Ability.CardAbility.Ignite){
            target.GetComponent<EnemyController>().igniteStack += 2;
        }

        if(card.ability.cardAbility == Ability.CardAbility.Poison){
            target.GetComponent<EnemyController>().poisonStack += 2;
        }
    }

    



    public void Hover(){
        DeckDrawing dd = FindObjectOfType<DeckDrawing>();

        transform.DOScale(new Vector3(1.5f,1.5f,1.5f), 0.5f);
        transform.DOMoveY(transform.position.y + 100, 0.5f);

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

    public void NoHover(){
        transform.DOScale(new Vector3(1.03905f, 1.03905f, 1.03905f), 0.5f);
        transform.DOMove(originalPosition, 0.5f);
        CloseCards.Clear();
        FarCards.Clear();

        DeckDrawing dd = FindObjectOfType<DeckDrawing>();
        for(int i = 0; i < dd.hand.Count; i++){
            dd.hand[i].transform.DOMove(dd.hand[i].originalPosition, 0.5f);
        }
    }
}
