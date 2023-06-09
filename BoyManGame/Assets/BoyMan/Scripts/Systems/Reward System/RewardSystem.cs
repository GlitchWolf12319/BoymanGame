using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RewardSystem : FindTargets
{
    public enum Panel1Rewards {Card, Gold}
    [Header("Reward for Boyman")]
    public Panel1Rewards panel1Rewards;
    public GameObject panel1;
    public Image panel1Icon;
    public TMP_Text icon1Text;
    public int gold1Ammount;

    public enum Panel2Rewards {Card, Gold}
    [Header("Reward for Jane")]
    public Panel2Rewards panel2Rewards;
    public GameObject panel2;
    public Image panel2Icon;
    public TMP_Text icon2Text;
    public int gold2Ammount;

    public enum Panel3Rewards {Card, Gold}
    [Header("Reward for Oslo")]
    public Panel3Rewards panel3Rewards;
    public GameObject panel3;
    public Image panel3Icon;
    public TMP_Text icon3Text;
    public int gold3Ammount;

    public enum Panel4Rewards {Card, Gold}
    [Header("Reward for Casper")]
    public Panel4Rewards panel4Rewards;
    public GameObject panel4;
    public Image panel4Icon;
    public TMP_Text icon4Text;
    public int gold4Ammount;

    public Sprite[] rewardIcons;
    public GameObject rewardScreen;
    public GameObject cardSelectionScreen;
    public List<GameObject> party;
    public CardTemplate[] cardsToChooseFrom;
    List<CardTemplate> cardsChosen = new List<CardTemplate>();
    public GameObject BlankCardPrefab;
    public GameObject AddedCardPrefab;
    public Transform[] cardSlots;
    public string currentClaimer;
    public Image tip;
    public int claimCounter;
    public int partyClaimer;
    


    void Start(){
        AssignValues();
        rewardScreen.transform.DOScale(new Vector3(2,2,2), 0.5f);
        tip.DOColor(Color.white, 0.2f).SetLoops(21, LoopType.Yoyo);
    }

    void AssignValues(){
        if(panel1Rewards == Panel1Rewards.Card){
            panel1Icon.sprite = rewardIcons[0];
        }

        if(panel1Rewards == Panel1Rewards.Gold){
            panel1Icon.sprite = rewardIcons[1];
            icon1Text.text = gold1Ammount.ToString();
        }

        if(panel2Rewards == Panel2Rewards.Card){
            panel2Icon.sprite = rewardIcons[0];
        }

        if(panel2Rewards == Panel2Rewards.Gold){
            panel2Icon.sprite = rewardIcons[1];
            icon2Text.text = gold2Ammount.ToString();
        }

        if(panel3Rewards == Panel3Rewards.Card){
            panel3Icon.sprite = rewardIcons[0];
        }

        if(panel3Rewards == Panel3Rewards.Gold){
            panel3Icon.sprite = rewardIcons[1];
            icon3Text.text = gold3Ammount.ToString();
        }

        if(panel4Rewards == Panel4Rewards.Card){
            panel4Icon.sprite = rewardIcons[0];
        }

        if(panel4Rewards == Panel4Rewards.Gold){
            panel4Icon.sprite = rewardIcons[1];
            icon4Text.text = gold4Ammount.ToString();
        }

        party = FindGoodChar();

        bool findBoyman = false;
        bool findJane = false;
        bool findOslo = false;
        bool findCasper = false;

        for(int i = 0; i < party.Count; i++){
            partyClaimer++;
            if(party[i].name.Contains("BoyMan")){
                findBoyman = true;
            }

            if(party[i].name.Contains("Jane")){
                findJane = true;
            }

            if(party[i].name.Contains("Oslo")){
                findOslo = true;
            }

            if(party[i].name.Contains("Casper")){
                findCasper = true;
            }
        }

        panel1.SetActive(findBoyman);
        panel2.SetActive(findJane);
        panel3.SetActive(findOslo);
        panel4.SetActive(findCasper);
   
    }


   public void BoymanButton(){
    claimCounter++;
    currentClaimer = "BoyMan";
    panel1.transform.DOScale(new Vector3(0,0,0), 0.5f);
        if(panel1Rewards == Panel1Rewards.Gold){
            GameObject.Find("BoyMan").GetComponent<CharacterController>().GiveCoins(gold1Ammount);
            panel1.transform.DOScale(new Vector3(0,0,0), 0.5f);

            if(claimCounter == partyClaimer){
                Leave();
            }
        }

        if(panel1Rewards == Panel1Rewards.Card){
            rewardScreen.transform.DOScale(new Vector3(0,0,0), 0.5f);
            cardSelectionScreen.transform.DOScale(new Vector3(2,2,2), 0.5f);
            DisplayCard();
        }
    }

    public void JaneButton(){
        claimCounter++;
        currentClaimer = "Jane";
        panel2.transform.DOScale(new Vector3(0,0,0), 0.5f);
        if(panel2Rewards == Panel2Rewards.Gold){
            GameObject.Find("Jane").GetComponent<CharacterController>().GiveCoins(gold2Ammount);
            panel2.transform.DOScale(new Vector3(0,0,0), 0.5f);

            if(claimCounter == partyClaimer){
                Leave();
            }
        }

        if(panel2Rewards == Panel2Rewards.Card){
            rewardScreen.transform.DOScale(new Vector3(0,0,0), 0.5f);
            cardSelectionScreen.transform.DOScale(new Vector3(2,2,2), 0.5f);
            DisplayCard();
        }
    }

    public void OsloButton(){
        claimCounter++;
        currentClaimer = "Oslo";
        panel3.transform.DOScale(new Vector3(0,0,0), 0.5f);
        if(panel3Rewards == Panel3Rewards.Gold){
            GameObject.Find("Oslo").GetComponent<CharacterController>().GiveCoins(gold3Ammount);
            panel3.transform.DOScale(new Vector3(0,0,0), 0.5f);

            if(claimCounter == partyClaimer){
                Leave();
            }
        }

        if(panel3Rewards == Panel3Rewards.Card){
            rewardScreen.transform.DOScale(new Vector3(0,0,0), 0.5f);
            cardSelectionScreen.transform.DOScale(new Vector3(2,2,2), 0.5f);
            DisplayCard();
        }
    }

    public void CasperButton(){
        claimCounter++;
        currentClaimer = "Casper";
        panel4.transform.DOScale(new Vector3(0,0,0), 0.5f);
        if(panel4Rewards == Panel4Rewards.Gold){
            GameObject.Find("Casper").GetComponent<CharacterController>().GiveCoins(gold2Ammount);
            panel4.transform.DOScale(new Vector3(0,0,0), 0.5f);

            if(claimCounter == partyClaimer){
                Leave();
            }
        }

        if(panel4Rewards == Panel4Rewards.Card){
            rewardScreen.transform.DOScale(new Vector3(0,0,0), 0.5f);
            cardSelectionScreen.transform.DOScale(new Vector3(2,2,2), 0.5f);
            DisplayCard();
        }
    }

    public void DisplayCard(){
        for(int i = 0; i < 3; i++){
            int randomCard = Random.Range(0, cardsToChooseFrom.Length);
            cardsChosen.Add(cardsToChooseFrom[randomCard]);
        }

        for(int i = 0; i < cardsChosen.Count; i++){
            GameObject cards = Instantiate(BlankCardPrefab);
            cards.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
            cards.GetComponent<AddCardToDeck>().index = i;
            cards.transform.SetParent(GameObject.Find("CardSelectionScreen").transform);
            cards.GetComponent<CardRender>().card = cardsChosen[i];
            cards.GetComponent<CardRender>().RenderCardInformation();
            cards.GetComponent<CardRender>().card = cardsChosen[i];
            cards.name = cardsChosen[i].name;
            cards.GetComponent<AddCardToDeck>().RS = this;
        }
   }

      public void AddCardToDeck(GameObject card, CardTemplate template){
    /////////////////////////- SHRINK CARDS -////////////////////////////////////////
        List<GameObject> cards = new List<GameObject>();

        foreach(var presentCards in FindObjectsOfType<AddCardToDeck>()){
            cards.Add(presentCards.gameObject);
        }

        for(int i = 0; i < cards.Count; i++){
            cards[i].transform.DOScale(new Vector3(0,0,0), 0.1f).OnComplete(() =>
            {
                Destroy(cards[i], 0.5f);
            });
            
        }

        cardsChosen.Clear();


        ///////////////////////- ADDING CARD TO PLAYER DECK -/////////////////////////////////////
        cardSelectionScreen.transform.DOScale(new Vector3(0, 0f, 0), 0.5f);
        rewardScreen.transform.DOScale(new Vector3(2f,2f,0), 0.5f);

        GameObject.Find(currentClaimer).GetComponent<NewDeckDrawing>().deck.Add(template);
        Canvas cardCanvas = GameObject.Find(currentClaimer).GetComponentInChildren<Canvas>();

        if(claimCounter == partyClaimer){
            Leave();
        }
   }

   public void Leave(){

        GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null){         
                Movement[] move = FindObjectsOfType<Movement>();
                foreach(Movement Move in move){
                Move.enabled = true;
            }
        }

        rewardScreen.transform.DOScale(new Vector3(0,0,0), 0.5f).OnComplete(() =>
        {
            Destroy(this.gameObject, 0.7f);
        });

        

        
   }

   public void Skip(){
        cardSelectionScreen.transform.DOScale(new Vector3(0,0,0), 0.5f);
        rewardScreen.transform.DOScale(new Vector3(2,2,2), 0.5f);

        List<GameObject> cards = new List<GameObject>();

        foreach(var presentCards in FindObjectsOfType<AddCardToDeck>()){
            cards.Add(presentCards.gameObject);
            Debug.Log(presentCards.name);
        }

        for(int i = 0; i < cards.Count; i++){
            cards[i].transform.DOScale(new Vector3(0,0,0), 0.1f).OnComplete(() =>
        {
            Destroy(cards[i], 0.5f);
        });
            
        }
        cardsChosen.Clear();

        if(claimCounter == partyClaimer){
            Leave();
        }
   }


}
