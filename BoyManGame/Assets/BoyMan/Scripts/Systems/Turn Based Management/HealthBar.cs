using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public CharacterStats CS;
    public CharacterController cc;
    private float newHealth;
    public TMP_Text HP;
    private float currentHealth;

    void Start(){
        currentHealth = slider.value; // store the current health value
    }

    public void Update(){
        slider.maxValue = CS.MaxHealth;
        newHealth = cc.health;
        HP.text = cc.health + " / " + cc.MaxHealth;

        // use lerp to smoothly transition between the current and new health values
        currentHealth = Mathf.Lerp(currentHealth, newHealth, Time.deltaTime * 10f);
        slider.value = currentHealth;
    }
}
