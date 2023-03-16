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
    private Vector3 originalPosition;

    void Start(){
        DrawCards(handSize);
        originalPosition = new Vector3(-1461f, -443, 0);
    }

    public void DrawCards(int ammount){
        float[] rotateValues = {13.3f, 8.9f, 0f, -8.9f, -13.3f};
        var deckSize = deck.Count;

        if(deckSize >= ammount){
            for(int i = 0; i < ammount; i++){
                int randomCard = Random.Range(0, deck.Count);
                hand.Add(deck[randomCard]);
                hand[i].transform.DOMove(cardPosition[i].position, 0.5f);
                hand[i].GetComponent<Card>().originalPosition = cardPosition[i].position;
                hand[i].GetComponent<Card>().index = i;
                hand[i].GetComponent<Card>().transform.DORotate(new Vector3(0,0,rotateValues[i]), 0.5f);
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

    public IEnumerator MoveToDiscardPile(Card card){
        card.transform.DOScale(new Vector3(1.03905f, 1.03905f, 1.03905f), 0.5f);
        var index = hand.IndexOf(card);
        hand.RemoveAt(index);
        discardPile.Add(card);
        card.transform.DOMoveX(card.transform.position.x + 2000, 0.5f);
        yield return new WaitForSeconds(3);
        card.transform.position = originalPosition;
    }

    public void MoveDeckToDiscardPile(){
        foreach(var card in hand){
            discardPile.Add(card);
            card.transform.DOMoveX(card.transform.position.x + 2000, 0.5f);
        }

        hand.Clear();
    }
}
