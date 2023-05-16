using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.UI;
[Serializable]
public class CardInformation
{
    public GameObject prefab;
    public List<CardTemplate> deck;
    public Sprite Border;
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


    void Start()
    {
        // Initialize the deck with cards from the CardInformation
        for (int i = 0; i < cardInfo.deck.Count; i++)
        {
            deck.Add(cardInfo.deck[i]);
        }
    }

    void Update()
    {
        // Update the UI text for deck and discard pile counts
        deckCountText.text = deck.Count.ToString();
        discardPileText.text = discardPile.Count.ToString();
    }

    public void DrawCards(int amount)
    {
        float[] rotateValues = { 13.3f, 8.9f, 0f, -8.9f, -13.3f };
        float[] speedValues = { 0.05f, 0.15f, 0.2f, 0.25f, 0.3f };

        int deckSize = deck.Count;

        if (deckSize >= amount)
        {
            // Draw cards from the deck
            for (int i = 0; i < amount; i++)
            {
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
        else
        {
            // If deck is empty, reshuffle the discard pile into the deck
            for (int i = 0; i < discardPile.Count; i++)
            {
                deck.Add(discardPile[i]);
            }

            discardPile.Clear();

            if (amount > deck.Count + discardPile.Count)
            {
                // Ensure the amount is within the available card count
                amount = deck.Count + discardPile.Count;
            }

            // Draw the remaining cards from the shuffled deck
            DrawCards(amount);
        }
    }


public void RotateCard(GameObject card, float rotateValue)
{
    // Rotate the card using the DOTween library
    card.transform.DORotate(new Vector3(0, 0, rotateValue), 0.1f);
}

public void MoveCardsToScreen(GameObject card, Transform slot, float Speed)
{
    // Move the card to the specified slot using DOTween
    card.GetComponent<Card>().originalPosition = slot.position;
    card.transform.DOMove(slot.position, Speed);
}

public IEnumerator MoveToDiscardPile(GameObject card)
{
    // Move the card to the discard pile
    discardPile.Add(card.GetComponent<Card>().card);
    hand.Remove(card);

    Vector3 middle = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
    card.transform.DOMove(middle, .1f);
    yield return new WaitForSeconds(0.1f);
    card.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.1f);

    yield return new WaitForSeconds(0.1f);
    card.transform.DORotate(new Vector3(0, 0, -145), 0.1f);
    yield return new WaitForSeconds(0.1f);

    card.transform.DOMove(new Vector3(middle.x + 3000, middle.y - 2000, middle.z), 1).OnComplete(() =>
    {
        // Perform actions when the card movement is complete
    });
}

public void ClearDeck()
{
    // Check if any cards are selected in the hand
    int counter = 0;
    for (int i = 0; i < transform.GetComponent<NewDeckDrawing>().hand.Count; i++)
    {
        if (transform.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().selected == true)
        {
            counter++;
        }
    }

    if (counter == 0)
    {
        // If no cards are selected, move the entire deck to the discard pile
        StartCoroutine(MoveDeckToDiscardPile());
    }
}

public IEnumerator MoveDeckToDiscardPile()
{
    // Move all cards in the hand to the discard pile
    List<GameObject> cards = new List<GameObject>(hand);

    foreach (var card in cards)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            card.GetComponent<Card>().canHover = false;
            card.GetComponent<Card>().disableHovering = true;
        }

        discardPile.Add(card.GetComponent<Card>().card);

        Vector3 middle = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        card.transform.DOMove(middle, .1f);
        yield return new WaitForSeconds(0.1f);
        card.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.1f);

        yield return new WaitForSeconds(0.1f);
        card.transform.DORotate(new Vector3(0, 0, -145), 0.1f);
        yield return new WaitForSeconds(0.1f);

        card.transform.DOMove(new Vector3(middle.x + 3000, middle.y - 2000, middle.z), 1).OnComplete(() =>
        {
            // Destroy the card when the movement is complete
            DestroyImmediate(card.gameObject, true);
        });
    }

    hand.Clear();
}

  public void AssignScripts(GameObject card, CardTemplate cardTemp)
{
    // Assign the CardTemplate to the CardRender component
    card.GetComponent<CardRender>().card = cardTemp;

    // Assign properties to the Card component
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
    card.GetComponent<Card>().speechBubblePrefab = Resources.Load("speechBubble_0") as GameObject;
    card.GetComponent<Card>().canUseColor = Color.blue;
    card.GetComponent<Card>().cantUseColor = Color.red;
    parent = this.gameObject;

    // Find the appropriate parent transform to set as the card's parent
    foreach (Transform children in this.transform)
    {
        if (children.name.Contains("CardCanvas"))
        {
            card.transform.SetParent(children);
        }
    }
}
}