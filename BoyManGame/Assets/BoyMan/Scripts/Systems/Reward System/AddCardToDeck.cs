using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCardToDeck : MonoBehaviour
{
    public RewardSystem RS;
    public int index;
   public void Clicked(){
        if(RS != null){
            RS.AddCardToDeck(this.gameObject, this.GetComponent<CardRender>().card);
        }
   }

   void Start(){
        transform.position = RS.cardSlots[index].transform.position;
   }
}
