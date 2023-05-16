using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCardToDeck : MonoBehaviour
{
    public RewardSystem RS; // Reference to the RewardSystem
    public int index; // Index used for positioning the card

    public void Clicked()
    {
        if (RS != null)
        {
            // Add the clicked card to the deck in the RewardSystem
            RS.AddCardToDeck(this.gameObject, this.GetComponent<CardRender>().card);
        }
    }

    void Start()
    {
        // Position the card at the corresponding card slot position in the RewardSystem
        transform.position = RS.cardSlots[index].transform.position;
    }
}




