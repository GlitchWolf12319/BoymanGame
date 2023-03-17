using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCardToDeck : MonoBehaviour
{
    public RewardSystemUI rsUI;
   public void Clicked(){
        if(rsUI != null){
            rsUI.AddCardToDeck(this.gameObject, this.GetComponent<CardRender>().card);
        }
   }
}
