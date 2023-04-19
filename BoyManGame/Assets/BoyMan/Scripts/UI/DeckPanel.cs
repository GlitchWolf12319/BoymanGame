using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.UI;

public class DeckPanel : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject currentPlayer;

    private void Start()
    {
        // Get the Layout Group component
        GridLayoutGroup layoutGroup = GetComponent<GridLayoutGroup>();

        // Loop through the deck and create a new card image for each card
        for (int i = 0; i < currentPlayer.GetComponent<NewDeckDrawing>().cardInfo.deck.Count; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            cardObject.transform.SetParent(transform);

            // Set the card image's sprite to the card's artwork
            Image cardImage = cardObject.GetComponent<Image>();
            Image cardBorder = cardObject.GetComponentInChildren<Image>();
            cardImage.sprite = currentPlayer.GetComponent<NewDeckDrawing>().cardInfo.deck[i].Artwork;
            cardBorder.sprite = currentPlayer.GetComponent<NewDeckDrawing>().cardInfo.Border;

            // Set the size of the card image to match the size of the artwork
            RectTransform cardRect = cardObject.GetComponent<RectTransform>();
            cardRect.sizeDelta = new Vector2(currentPlayer.GetComponent<NewDeckDrawing>().cardInfo.deck[i].Artwork.rect.width, currentPlayer.GetComponent<NewDeckDrawing>().cardInfo.deck[i].Artwork.rect.height);
            

            // Add spacing between the cards
            if (i > 0)
            {
                cardRect.offsetMin = new Vector2(layoutGroup.spacing.y, cardRect.offsetMin.y);
            }
        }
    }
}
