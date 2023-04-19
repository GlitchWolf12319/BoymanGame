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
    public Sprite DefaultBorder;

    void Start(){
        RenderCardInformation();
    }

    public void RenderCardInformation(){
        APCost.text = card.APCost.ToString();
        CardName.text = card.Name;
        CardDescription.text = card.Description;
        ArtworkImage.sprite = card.Artwork;

        if(transform.tag != "RewardCard"){
            BorderImage.sprite = transform.GetComponent<Card>().caster.GetComponent<NewDeckDrawing>().cardInfo.Border;
        }
        else{
            BorderImage.sprite = DefaultBorder;
        }
        

    }

}
