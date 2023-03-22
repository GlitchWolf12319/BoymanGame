using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DeckDrawing : MonoBehaviour
{
    public List<Card> deck;
    public TMP_Text deckText;
    public List<Card> discardPile;
    public TMP_Text discardText;
    public List<Card> hand;
    [SerializeField] private List<Transform> cardPosition;
    private const int handSize = 5;
    private Vector3 originalPosition;

    void Start(){
        //DrawCards(handSize);
        originalPosition = new Vector3(-1461f, -443, 0);
    }

    void Update(){
        deckText.text = deck.Count.ToString();
        discardText.text = discardPile.Count.ToString();
    }

    public void DrawCards(int ammount){
        Debug.Log("Deck Drawing");        
        float[] rotateValues = {13.3f, 8.9f, 0f, -8.9f, -13.3f};
        float[] speedValues = {0.05f, 0.15f, 0.2f, 0.25f, 0.3f};
        var deckSize = deck.Count;

        if(deckSize >= ammount){
            for(int i = 0; i < ammount; i++){
                int randomCard = Random.Range(0, deck.Count);
                hand.Add(deck[randomCard]);
                hand[i].gameObject.SetActive(true);
                hand[i].CheckCurrentAPAgainstCard();
                hand[i].transform.DOMove(cardPosition[i].position, speedValues[i]);
                hand[i].GetComponent<Card>().originalPosition = cardPosition[i].position;
                hand[i].GetComponent<Card>().index = i;
                hand[i].GetComponent<Card>().transform.DORotate(new Vector3(0,0,rotateValues[i]), 0.1f);
                hand[i].transform.SetSiblingIndex(i);
                hand[i].GetComponent<Card>().canHover = true;
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


        for(int i = 0; i < discardPile.Count; i++){
            discardPile[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < deck.Count; i++){
            deck[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator MoveToDiscardPile(Card card){
        card.transform.DOScale(new Vector3(1.03905f, 1.03905f, 1.03905f), 0.5f);
        var index = hand.IndexOf(card);
        hand.RemoveAt(index);
        discardPile.Add(card);
        //card.transform.DOMoveX(card.transform.position.x + 2000, 0.5f);
        yield return new WaitForSeconds(1);
        //card.transform.position = originalPosition;
    }

    public void moveDeckToDiscard(){
        int counter = 0;
        for(int i = 0; i < hand.Count; i++){
            if(hand[i].GetComponent<Card>().selected == true){
                counter++;
            }
        }

        if(counter == 0){
            Debug.Log("canSkip");
            //StartCoroutine(EndTurn());
            StartCoroutine(MoveDeckToDiscardPile());
        }
        
    }

    public IEnumerator MoveDeckToDiscardPile(){
        foreach(var card in hand){
            for(int i = 0; i < hand.Count; i++){
                card.GetComponent<Card>().canHover = false;
            }
            discardPile.Add(card);

            // card.transform.DOMove(new Vector3(card.transform.position.x + 2000, card.transform.position.y - 2000, card.transform.position.z), 1).OnComplete(() =>
            // {
            //     card.transform.DOScale(new Vector3(1.03905f, 1.03905f, 1.03905f), 0.5f);
            //     card.transform.eulerAngles = new Vector3(0,0,0);
            //     card.transform.position = new Vector3(card.GetComponent<Card>().originalPosition.x - 1600, card.GetComponent<Card>().originalPosition.y, card.GetComponent<Card>().originalPosition.z);
            // });
            // card.transform.DOMoveX(card.transform.position.x + 2000, 0.5f);


            Debug.Log("deck animation");
            Vector3 middle = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            card.transform.DOMove(middle, .1f);
            yield return new WaitForSeconds(0.1f);
            card.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.1f);
            //card.GetComponent<Card>().trail.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            card.transform.DORotate(new Vector3(0, 0, -145), 0.1f);
            yield return new WaitForSeconds(0.1f);
            //card.transform.DOMove(new Vector3(card.transform.position.x + 2000, card.transform.position.y - 2000, card.transform.position.z), 1);
   
            card.transform.DOMove(new Vector3(card.transform.position.x + 2000, card.transform.position.y - 2000, card.transform.position.z), 1).OnComplete(() =>
            {
                card.transform.DOScale(new Vector3(1.03905f, 1.03905f, 1.03905f), 0.5f);
                card.transform.eulerAngles = new Vector3(0,0,0);
                card.transform.position = new Vector3(card.GetComponent<Card>().originalPosition.x - 1600, card.GetComponent<Card>().originalPosition.y, card.GetComponent<Card>().originalPosition.z);
            });
        }

        hand.Clear();
    }
}
