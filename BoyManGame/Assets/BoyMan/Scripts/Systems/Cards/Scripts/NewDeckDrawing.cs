using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.UI;

[Serializable]
public class CardInformation{
    public GameObject prefab;
    public List<CardTemplate> deck;
}

public class NewDeckDrawing : MonoBehaviour
{
    public CardInformation cardInfo;
    //public GameObject CardPrefab;
    public List<CardTemplate> deck;
    public List<GameObject> hand;
    public List<CardTemplate> discardPile;

    public GameObject parent;
    public Transform cardSpawnPosition;
    public Transform[] spawnSlots;

    public TMP_Text deckCountText;
    public TMP_Text discardPileText;


    void Start(){
        for(int i = 0; i < cardInfo.deck.Count; i++){
            deck.Add(cardInfo.deck[i]);
        }
    }

    void Update(){
        deckCountText.text = deck.Count.ToString();
        discardPileText.text = discardPile.Count.ToString();
    }

    public void DrawCards(int ammount){

        float[] rotateValues = {13.3f, 8.9f, 0f, -8.9f, -13.3f};
        float[] speedValues = {0.05f, 0.15f, 0.2f, 0.25f, 0.3f};

        var deckSize = deck.Count;
        //var deckCopy = new List<CardTemplate>(cardInfo.deck);

        if(deckSize >= ammount){

            for(int i = 0; i < ammount; i++){
                int randomCard = UnityEngine.Random.Range(0, deck.Count);
                GameObject runtimeCard = Instantiate(cardInfo.prefab, cardSpawnPosition.position, Quaternion.identity);
                runtimeCard.GetComponent<Card>().index = i;
                hand.Add(runtimeCard);
                AssignScripts(runtimeCard, deck[randomCard]);
                RotateCard(runtimeCard, rotateValues[i]);
                MoveCardsToScreen(runtimeCard, spawnSlots[i], speedValues[i]);

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

public void RotateCard(GameObject card, float rotateValue){
    card.transform.DORotate(new Vector3(0,0, rotateValue), 0.1f);
}

    public void MoveCardsToScreen(GameObject card, Transform slot, float Speed){
        card.GetComponent<Card>().originalPosition = slot.position;
        card.transform.DOMove(slot.position, Speed);
    }

    public IEnumerator MoveToDiscardPile(GameObject card){
            discardPile.Add(card.GetComponent<Card>().card);
            hand.Remove(card);

            Vector3 middle = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            card.transform.DOMove(middle, .1f);
            yield return new WaitForSeconds(0.1f);
            card.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.1f);
            //card.GetComponent<Card>().trail.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            card.transform.DORotate(new Vector3(0, 0, -145), 0.1f);
            yield return new WaitForSeconds(0.1f);
            //card.transform.DOMove(new Vector3(card.transform.position.x + 2000, card.transform.position.y - 2000, card.transform.position.z), 1);
            //InHandCard.ResetCard();
            card.transform.DOMove(new Vector3(card.transform.position.x + 2000, card.transform.position.y - 2000, card.transform.position.z), 1).OnComplete(() =>
            {
                //DestroyImmediate(card.gameObject, true);
            
            });
    }

    public void ClearDeck(){
        StartCoroutine(MoveDeckToDiscardPile());
    }

    public IEnumerator MoveDeckToDiscardPile(){

        List<GameObject> cards = new List<GameObject>(hand);

        foreach(var card in cards){
            
            for(int i = 0; i < hand.Count; i++){
                card.GetComponent<Card>().canHover = false;
                card.GetComponent<Card>().disableHovering = true;
            }

            discardPile.Add(card.GetComponent<Card>().card);

            Vector3 middle = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            card.transform.DOMove(middle, .1f);
            yield return new WaitForSeconds(0.1f);
            card.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.1f);
            //card.GetComponent<Card>().trail.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            card.transform.DORotate(new Vector3(0, 0, -145), 0.1f);
            yield return new WaitForSeconds(0.1f);
            //card.transform.DOMove(new Vector3(card.transform.position.x + 2000, card.transform.position.y - 2000, card.transform.position.z), 1);
            //card.ResetCard();
            card.transform.DOMove(new Vector3(card.transform.position.x + 2000, card.transform.position.y - 2000, card.transform.position.z), 1).OnComplete(() =>
            {
                DestroyImmediate(card.gameObject, true);
            });
        }

        hand.Clear();
    }

    

    public void AssignScripts(GameObject card, CardTemplate cardTemp){
        card.GetComponent<CardRender>().card = cardTemp;


        card.GetComponent<Card>().card = cardTemp;
        card.GetComponent<Card>().shieldIconPrefab = Resources.Load("BlockIcon") as GameObject;
        card.GetComponent<Card>().selected = false;
        card.GetComponent<Card>().canSelectTarget = false;
        card.GetComponent<Card>().onHover = false;
        card.GetComponent<Card>().Drag = false;
        card.GetComponent<Card>().Arrow = false;
        card.GetComponent<Card>().canHover = true;
        card.GetComponent<Card>().canSelect = true;
        card.GetComponent<Card>().disableHovering = false;
        card.GetComponent<Card>().caster = this.gameObject;
        // card.GetComponent<Card>().arrow = Resources.Load("Arrow") as GameObject;
        card.GetComponent<Card>().speechBubblePrefab = Resources.Load("speechBubble_0") as GameObject;
        card.GetComponent<Card>().canUseColor = Color.blue;
        card.GetComponent<Card>().cantUseColor = Color.red;
        parent = this.gameObject;

        foreach(Transform children in this.transform){
            if(children.name.Contains("CardCanvas")){
                card.transform.SetParent(children);
            }
        }
        


    }   
}
