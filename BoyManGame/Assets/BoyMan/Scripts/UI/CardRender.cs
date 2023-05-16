using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRender : MonoBehaviour
{
    public CardTemplate card;  // Reference to the CardTemplate scriptable object
    public Text APCost;  // Reference to the text component for displaying AP cost
    public Text CardName;  // Reference to the text component for displaying card name
    public Text CardDescription;  // Reference to the text component for displaying card description
    public Image ArtworkImage;  // Reference to the image component for displaying card artwork
    public Image BorderImage;  // Reference to the image component for displaying card border
    public Sprite DefaultBorder;  // Default border sprite

    void Start()
    {
        RenderCardInformation();  // Render the card information on start
    }

    public void RenderCardInformation()
    {
        APCost.text = card.APCost.ToString();  // Set the AP cost text to the value from the CardTemplate
        CardName.text = card.Name;  // Set the card name text to the value from the CardTemplate
        CardDescription.text = card.Description;  // Set the card description text to the value from the CardTemplate
        ArtworkImage.sprite = card.Artwork;  // Set the card artwork image to the sprite from the CardTemplate

        if (transform.tag != "RewardCard")
        {
            // If the card is not a reward card, set the border image to the border sprite from the caster's NewDeckDrawing script
            BorderImage.sprite = transform.GetComponent<Card>().caster.GetComponent<NewDeckDrawing>().cardInfo.Border;
        }
        else
        {
            // If the card is a reward card, set the border image to the default border sprite
            BorderImage.sprite = DefaultBorder;
        }
    }
}