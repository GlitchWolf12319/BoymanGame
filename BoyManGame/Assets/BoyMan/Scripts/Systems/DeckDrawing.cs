using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeckDrawing : MonoBehaviour
{
    [SerializeField] private List<Card> deck;
    [SerializeField] private List<Card> discardPile;
    public List<Card> hand;
    [SerializeField] private List<Transform> cardPosition;
    private const int handSize = 5;

    void Start(){
        DrawCards(handSize);
    }

    public void DrawCards(int ammount){

        var deckSize = deck.Count;

        if(deckSize >= ammount){
            for(int i = 0; i < ammount; i++){
                int randomCard = Random.Range(0, deck.Count);
                hand.Add(deck[randomCard]);
                hand[i].transform.DOMove(cardPosition[i].position, 0.5f);
                hand[i].GetComponent<Card>().originalPosition = cardPosition[i].position;
                hand[i].GetComponent<Card>().index = i;
                hand[i].transform.SetSiblingIndex(i);
                deck.RemoveAt(randomCard);
            }   
        }
        else{
            for(int i = 0; i < discardPile.Count; i++){
                deck.Add(discardPile[i]);
            }

            discardPile.Clear();

            if(ammount > deck.Count + discardPile.Count){
                ammount = deck.Count + discardPile.Count;
            }
            DrawCards(ammount);
        }
    }

    public void MoveToDiscardPile(Card card){
        var index = hand.IndexOf(card);
        hand.RemoveAt(index);
        discardPile.Add(card);
        card.transform.DOMoveX(card.transform.position.x + 2000, 0.5f);
    }

    public void MoveDeckToDiscardPile(){
        foreach(var card in hand){
            discardPile.Add(card);
            card.transform.DOMoveX(card.transform.position.x + 2000, 0.5f);
        }

        hand.Clear();
    }
}
