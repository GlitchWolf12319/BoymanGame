using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Card", menuName = "CardTemplate")]
public class CardTemplate : ScriptableObject
{
        
        public string Name;
        public int APCost;
        public string Description;
        public Sprite Border;
        public Sprite Artwork;
        public CardType[] ability;

        public enum AttackMethod {Drag, Arrow}
        public AttackMethod attackMethod;
}
