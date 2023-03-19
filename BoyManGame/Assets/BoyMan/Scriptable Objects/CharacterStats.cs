using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "stats", menuName = "CharacterStat")]
public class CharacterStats : ScriptableObject
{
    public string CharacterName;
    public int MaxHealth;
    public int startingAP;
    public int MaxAP;
}
