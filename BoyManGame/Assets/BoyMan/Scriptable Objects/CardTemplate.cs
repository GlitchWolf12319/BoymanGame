using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Card", menuName = "CardTemplate")]
public class CardTemplate : ScriptableObject
{
    public string Name;  // Name of the card
    public int APCost;  // Ability Point cost of the card
    public string Description;  // Description of the card
    public Sprite Border;  // Border sprite for the card
    public Sprite Artwork;  // Artwork sprite for the card
    public CardType[] ability;  // Array of card types representing the abilities of the card

    public enum AttackMethod { Drag, Arrow }  // Enum representing different attack methods
    public AttackMethod attackMethod;  // Selected attack method for the card

    public GameObject AttackEffect;  // Attack effect game object associated with the card
    public AudioClip cardSoundEffect;  // Sound effect clip for the card
}