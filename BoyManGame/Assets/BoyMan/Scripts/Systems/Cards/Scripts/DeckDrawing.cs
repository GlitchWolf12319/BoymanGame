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

    void Start()
    {
        originalPosition = new Vector3(-1461f, -443, 0);
    }

    void Update()
    {
        // Update the text displaying the number of cards in the deck and discard pile
        deckText.text = deck.Count.ToString();
        discardText.text = discardPile.Count.ToString();
    }

    public void DrawCards(int ammount)
    {
        Debug.Log("Deck Drawing");        
        float[] rotateValues = {13.3f, 8.9f, 0f, -8.9f, -13.3f};
        float[] speedValues = {0.05f, 0.15f, 0.2f, 0.25f, 0.3f};
        var deckSize = deck.Count;

        if (deckSize >= ammount)
        {
            for (int i = 0; i < ammount; i++)
            {
                // Select a random card from the deck
                int randomCard = Random.Range(0, deck.Count);
                // Add the selected card to the hand
                hand.Add(deck[randomCard]);
                // Activate the card's game object
                hand[i].gameObject.SetActive(true);
                // Move the card to the designated card position using a Tween animation
                hand[i].transform.DOMove(cardPosition[i].position, speedValues[i]);
                // Set the card's original position and index
                hand[i].GetComponent<Card>().originalPosition = cardPosition[i].position;
                hand[i].GetComponent<Card>().index = i;
                // Rotate the card
                hand[i].GetComponent<Card>().transform.DORotate(new Vector3(0,0,rotateValues[i]), 0.1f);
                // Set the card's sibling index
                hand[i].transform.SetSiblingIndex(i);
                // Enable hovering for the card
                hand[i].GetComponent<Card>().canHover = true;
                // Remove the selected card from the deck
                deck.RemoveAt(randomCard);
            }   
        }
        else
        {
            // If the deck does not have enough cards, shuffle the discard pile back into the deck
            for (int i = 0; i < discardPile.Count; i++)
            {
                deck.Add(discardPile[i]);
            }
            discardPile.Clear();

            // Limit the amount of cards to draw to the total number of cards available
            if (ammount > deck.Count + discardPile.Count)
            {
                ammount = deck.Count + discardPile.Count;
            }
            
            // Redraw the cards with the new amount
            DrawCards(ammount);
        }

        // Deactivate all cards in the discard pile
        for (int i = 0; i < discardPile.Count; i++)
        {
            discardPile[i].gameObject.SetActive(false);
        }

        // Deactivate all cards in the deck
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].gameObject.SetActive(false);
        }
    }
   public IEnumerator MoveToDiscardPile(Card card)
{
    // Scale down the card using a Tween animation
    card.transform.DOScale(new Vector3(1.03905f, 1.03905f, 1.03905f), 0.5f);
    var index = hand.IndexOf(card);
    hand.RemoveAt(index);
    discardPile.Add(card);
    yield return new WaitForSeconds(1);
}

public void moveDeckToDiscard()
{
    int counter = 0;
    // Check if any cards in the hand are selected
    for (int i = 0; i < hand.Count; i++)
    {
        if (hand[i].GetComponent<Card>().selected == true)
        {
            counter++;
        }
    }

    if (counter == 0)
    {
        Debug.Log("canSkip");
        // Start the coroutine to move the deck to the discard pile
        StartCoroutine(MoveDeckToDiscardPile());
    }
}

public IEnumerator MoveDeckToDiscardPile()
{
    foreach (var card in hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            // Disable hovering for all cards in the hand
            card.GetComponent<Card>().canHover = false;
        }

        discardPile.Add(card);

        Vector3 middle = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        // Move the card to the center of the screen using a Tween animation
        card.transform.DOMove(middle, .1f);
        yield return new WaitForSeconds(0.1f);
        // Scale down the card using a Tween animation
        card.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.1f);
        yield return new WaitForSeconds(0.1f);
        // Rotate the card using a Tween animation
        card.transform.DORotate(new Vector3(0, 0, -145), 0.1f);
        yield return new WaitForSeconds(0.1f);
        // Move the card to an off-screen position using a Tween animation
        card.transform.DOMove(new Vector3(card.transform.position.x + 2000, card.transform.position.y - 2000, card.transform.position.z), 1)
            .OnComplete(() =>
            {
                // Deactivate the card and reset its scale, rotation, and position
                card.gameObject.SetActive(false);
                card.transform.DOScale(new Vector3(1.03905f, 1.03905f, 1.03905f), 0.5f);
                card.transform.eulerAngles = new Vector3(0, 0, 0);
                card.transform.position = new Vector3(card.GetComponent<Card>().originalPosition.x - 3000, card.GetComponent<Card>().originalPosition.y, card.GetComponent<Card>().originalPosition.z);
            });
    }

    // Clear the hand
    hand.Clear();
}
}