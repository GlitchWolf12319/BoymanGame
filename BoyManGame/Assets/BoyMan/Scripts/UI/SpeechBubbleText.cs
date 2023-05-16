using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubbleText : MonoBehaviour
{
    [SerializeField] private TMP_Text Messagetext;  // Reference to the TMP_Text component for displaying the message
    [SerializeField] private float typingSpeed;  // The speed at which the message is typed

    void Start()
    {
        StartCoroutine(Type("Not Enough AP"));  // Start the typing animation with the given message
    }

    IEnumerator Type(string message)
    {
        yield return new WaitForSeconds(0.5f);  // Delay before starting the typing animation

        foreach (char letter in message.ToCharArray())
        {
            Messagetext.text += letter;  // Add each letter to the message text
            yield return new WaitForSeconds(typingSpeed);  // Delay between typing each letter
        }
    }
}