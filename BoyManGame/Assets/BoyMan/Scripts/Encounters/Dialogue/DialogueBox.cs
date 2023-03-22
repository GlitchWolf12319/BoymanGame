using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public Image imageDisplay;
    public string[] sentences;
    public Sprite[] images;
    public Image Textbox;
    public Image Panel;
    private int index;
    public float typingSpeed;
    private bool isTyping = false;
    public float fadeSpeed = 1.0f;
   [Header("Use this if need player")]
    [SerializeField,HideInInspector] private GameObject player;
    public CanvasGroup canvasGroup;

   void Start(){
      imageDisplay.sprite = images[index];
       StartCoroutine(Type());

    //Fade in images and text
    imageDisplay.canvasRenderer.SetAlpha(0.0f);
    imageDisplay.CrossFadeAlpha(1.0f, fadeSpeed, false);

    Panel.canvasRenderer.SetAlpha(0.0f);
    Panel.CrossFadeAlpha(1.0f, fadeSpeed, false);

    Textbox.canvasRenderer.SetAlpha(0.0f);
    Textbox.CrossFadeAlpha(1.0f, fadeSpeed, false);

   }

IEnumerator Type()
{
    yield return new WaitForSeconds(typingSpeed);
    isTyping = true;
    foreach (char letter in sentences[index].ToCharArray())
    {
        textDisplay.text += letter;
        yield return new WaitForSeconds(typingSpeed);
    }
    isTyping = false;
}

public void NextSentence()
{
    if (!isTyping && index < sentences.Length - 1)
    {
        index++;
        textDisplay.text = "";
        imageDisplay.sprite = images[index];
        StopAllCoroutines();
        StartCoroutine(Type());
    }
    else if (!isTyping)
    {
        // End of dialogue
        StartCoroutine(FadeOut());
    }
}

IEnumerator FadeOut()
{
        yield return new WaitForSeconds(0.1f);
        
    // Fade out images and text
    imageDisplay.CrossFadeAlpha(0.0f, 1f, false);
    Panel.CrossFadeAlpha(0.0f, 1f, false);
    Textbox.CrossFadeAlpha(0.0f, 1f, false);
    player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){         
    //  Set the spawn chance of room 0 to 100%
         MoveRight[] move = FindObjectsOfType<MoveRight>();
         foreach(MoveRight Move in move){
            Move.Move();
         }

         CharacterController[] cc = FindObjectsOfType<CharacterController>();
         foreach(CharacterController charController in cc){
            charController.anim.SetBool("isIdle", false);
         }
    }
    // Wait for fade to complete

    // Hide the dialogue box
    canvasGroup.alpha = 0.0f;
    canvasGroup.interactable = false;
    canvasGroup.blocksRaycasts = false;
      yield return new WaitForSeconds(1.1f);
      Destroy(gameObject);
}

void Update()
{
    if (Input.GetMouseButtonDown(0))
    {
        NextSentence();
    }
}
}