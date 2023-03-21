using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubbleText : MonoBehaviour
{

    [SerializeField] private TMP_Text Messagetext;
    [SerializeField] private float typingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Type("Not Enough AP"));
    }

    IEnumerator Type(string message){

        yield return new WaitForSeconds(0.5f);
        foreach (char letter in message.ToCharArray())
        {
            Messagetext.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
