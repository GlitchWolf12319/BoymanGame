using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RewardSystemUI : MonoBehaviour
{
   public enum Button1 {Gold, Card}
   public Button1 button1Option;
   public GameObject Button1Text;

   public enum Button2 {Gold, Card}
   public Button2 button2Option;
   public GameObject Button2Text;

   public GameObject caster;
   public GameObject cardScreen;

   public CardTemplate[] cardsToChooseFrom;
   List<CardTemplate> cardsChosen = new List<CardTemplate>();
   public Transform[] cardSlots;
   public GameObject BlankCardPrefab;
   public GameObject AddedCardPrefab;

    void Start(){
        AssingValues();
    }

   public void AssingValues(){
        Button1Text.GetComponentInChildren<TMP_Text>().text = button1String();
        Button2Text.GetComponentInChildren<TMP_Text>().text = button2String();
   }

   public void Button1Button(){
        if(button1Option == Button1.Gold){
            caster.GetComponent<CharacterController>().GiveCoins(10);
            Button1Text.transform.DOScale(new Vector3(0,0,0), 0.5f);
            Destroy(Button1Text, 0.6f);
        }

        if(button1Option == Button1.Card){
            //caster.GetComponent<CharacterController>().GiveCoins(10);
            Button1Text.transform.DOScale(new Vector3(0,0,0), 0.5f);
            transform.DOScale(new Vector3(0,0,0), 0.5f);
            Destroy(Button1Text, 0.6f);
            cardScreen.transform.DOScale(new Vector3(4, 1.7f, 0), 0.5f);
            Invoke("DisplayCard", 0.6f);
        }
   }

   public void Button2Button(){
        if(button2Option == Button2.Gold){
            caster.GetComponent<CharacterController>().GiveCoins(10);
            Button2Text.transform.DOScale(new Vector3(0,0,0), 0.5f);
            Destroy(Button2Text, 0.6f);
        }
   }

   public void DisplayCard(){
        for(int i = 0; i < 3; i++){
            int randomCard = Random.Range(0, cardsToChooseFrom.Length);
            cardsChosen.Add(cardsToChooseFrom[randomCard]);
        }

        for(int i = 0; i < cardsChosen.Count; i++){
            GameObject cards = Instantiate(BlankCardPrefab);
            cards.transform.DOScale(new Vector3(0.4f, 1f, 1.03905f), 0.5f);
            cards.transform.DOMove(cardSlots[i].transform.position, 0.5f);
            cards.GetComponent<CardRender>().card = cardsChosen[i];
            cards.GetComponent<CardRender>().RenderCardInformation();
            cards.GetComponent<CardRender>().card = cardsChosen[i];
            cards.name = cardsChosen[i].name;
            cards.transform.SetParent(GameObject.Find("CardOptions").transform);
            cards.GetComponent<AddCardToDeck>().rsUI = this;
        }
   }

   public void AddCardToDeck(GameObject card, CardTemplate template){
    /////////////////////////- SHRINK CARDS -////////////////////////////////////////
        List<GameObject> cards = new List<GameObject>();

        foreach(var presentCards in FindObjectsOfType<AddCardToDeck>()){
            cards.Add(presentCards.gameObject);
        }

        for(int i = 0; i < cards.Count; i++){
            cards[i].transform.DOScale(new Vector3(0,0,0), 0.1f);
            Destroy(cards[i], 0.5f);
        }


        ///////////////////////- ADDING CARD TO PLAYER DECK -/////////////////////////////////////
        cardScreen.transform.DOScale(new Vector3(0, 0f, 0), 0.5f);
        transform.DOScale(new Vector3(1.5f,1.5f,0), 0.5f);
        GameObject addedCard = Instantiate(AddedCardPrefab);
        addedCard.GetComponent<CardRender>().card = template;
        addedCard.GetComponent<CardRender>().RenderCardInformation();
        addedCard.GetComponent<Card>().card = template;
        addedCard.name = template.name;
        addedCard.transform.SetParent(GameObject.FindGameObjectWithTag("BoymanCards").transform);
        GameObject.Find("boyman").GetComponent<DeckDrawing>().deck.Add(addedCard.GetComponent<Card>());

   }

   public void Leave(){
        transform.DOScale(new Vector3(0,0,0), 0.5f);
        Destroy(transform.parent.gameObject, 0.7f);
   }

   public string button1String(){

        if(button1Option == Button1.Gold){
            return "Recieve 10 Gold";
        }
        else if(button1Option == Button1.Card){
            return "Recieve a free Card";
        }
        else{
            return null;
        }
   }

   public string button2String(){

        if(button2Option == Button2.Gold){
            return "Recieve 10 Gold";
        }
        else if(button2Option == Button2.Card){
            return "Recieve a free Card";
        }
        else{
            return null;
        }
   }
}
