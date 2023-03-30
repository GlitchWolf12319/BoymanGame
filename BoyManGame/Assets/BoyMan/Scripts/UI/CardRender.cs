using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRender : MonoBehaviour
{
    public CardTemplate card;
    public Text APCost;
    public Text CardName;
    public Text CardDescription;
    public Image ArtworkImage;
    public Image BorderImage;

    void Start(){
        RenderCardInformation();
    }

    public void RenderCardInformation(){
        APCost.text = card.APCost.ToString();
        CardName.text = card.Name;
        CardDescription.text = card.Description;
        ArtworkImage.sprite = card.Artwork;
        BorderImage.sprite = card.Border;

    }

}
