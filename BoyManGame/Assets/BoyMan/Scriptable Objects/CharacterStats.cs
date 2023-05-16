using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "stats", menuName = "CharacterStat")]
public class CharacterStats : ScriptableObject
{
    // The name of the character
    public string CharacterName;

    // The maximum health of the character
    public int MaxHealth;

    // The initial amount of action points (AP) for the character
    public int startingAP;

    // The maximum amount of action points (AP) the character can have
    public int MaxAP;
}
