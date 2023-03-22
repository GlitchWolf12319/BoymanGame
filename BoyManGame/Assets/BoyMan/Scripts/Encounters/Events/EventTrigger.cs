using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class EventTrigger : MonoBehaviour
{
    [Header("Edit Title Text")]
    [SerializeField] private string titleInfo;
    [Header("Use this if spawning new map")]
    [SerializeField, HideInInspector] private GameObject spawnRoom;
    [Header("Use this if need player")]
    [SerializeField, HideInInspector] private GameObject player;
    [Header("Use this if need Transtion")]
    [SerializeField] private GameObject Transiton;
    [Header("Edit Description Text")]
    public ColorCodedTextField[] DescriptionFields;
    [Header("Edit Option One Text")]
    public ColorCodedTextField[] Option_One_Fields;
    [Header("Edit Option Two Text")]
    public ColorCodedTextField[] Option_Two_Fields;
    [Header("Gameplay Variables Button 1")]
    [SerializeField] private bool Changemap;
    [SerializeField, Range(0, 100)] private int B1_healProbability = 25;
    [SerializeField, Range(0, 100)] private int B1_healAmount = 100;
    [SerializeField, Range(0, 100)] private int B1_DamageProbability = 25;
    [SerializeField, Range(0, 100)] private int B1_damage = 10;
    [Header("Gameplay Variables Button 2")]
    [SerializeField, Range(0, 100)] private int B2_healProbability = 25;
    [SerializeField, Range(0, 100)] private int B2_healAmount = 10;
    [SerializeField, Range(0, 100)] private int B2_damageProbability = 25;
    [SerializeField, Range(0, 100)] private int B2_damage = 10;
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI button1Text;
    [SerializeField] private TextMeshProUGUI button2Text;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image image;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private TextMeshProUGUI titleText;
    [Header("Animation")]
    [SerializeField, HideInInspector] private float fadeDuration = 1.0f;

    // Color-coded text fields for description and button text
    [System.Serializable]
    public class ColorCodedTextField
    {
        public string text;
        public Color color;
        public int startIndex;
        public int length;
    }

    public void Start()
    {
        panel = GameObject.Find("Panel");
        button1Text = GameObject.Find("Button1Text").GetComponent<TextMeshProUGUI>();
        button2Text = GameObject.Find("Button2Text").GetComponent<TextMeshProUGUI>();
        descriptionText = GameObject.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        image = GameObject.Find("Image").GetComponent<Image>();
        button1 = GameObject.Find("Button1").GetComponent<Button>();
        button2 = GameObject.Find("Button2").GetComponent<Button>();
        titleText = GameObject.Find("TitleText").GetComponent<TextMeshProUGUI>();
         player = GameObject.FindGameObjectWithTag("Player");
    }

public void OnButton1Click()
{
  

    int chance = Random.Range(1, 101);
    if (chance <= B1_DamageProbability)
    {
        Debug.Log("Button 1 deals " + B1_damage + " damage.");
        B1_healAmount -= B1_damage;

        CharacterController[] chars = GameObject.FindObjectsOfType<CharacterController>();
        foreach(CharacterController cc in chars){
            if(cc.gameObject.tag == "Player"){
                Debug.Log(cc.gameObject.name);
                cc.TakeDamage(B1_damage, "Damage");
            }

            cc.anim.SetBool("isIdle", false);
        }

        }

    
    else if (chance <= B1_DamageProbability + B1_healProbability)
    {
        Debug.Log("Button 1 heals " + B1_healAmount + " health.");
         CharacterController[] chars = GameObject.FindObjectsOfType<CharacterController>();
        foreach(CharacterController cc in chars){
            if(cc.gameObject.tag == "Player"){
                cc.Heal(B1_healAmount);
            }

            cc.anim.SetBool("isIdle", false);
        }


    }
    else if(Changemap)
    {
        Instantiate(Transiton);
        
         // Find the GameObject with the "SpawnNextRoom" tag
        spawnRoom = GameObject.FindGameObjectWithTag("SpawnNextRoom");
        if(spawnRoom != null){          
    //  Set the spawn chance of room 0 to 100%
         spawnRoom.GetComponent<SpawnRoom>().SetSpawnChanceTo100(0);
      
        }   
    }
    else{
  Debug.Log("Button 1 does nothing.");
        CharacterController[] chars = GameObject.FindObjectsOfType<CharacterController>();
        foreach(CharacterController cc in chars){
            cc.anim.SetBool("isIdle", false);
        }
    }

    StartCoroutine(FadeOutPanel());
}

 public void OnButton2Click()
{
     
    int chance = Random.Range(1, 101);
    if (chance <= B2_damageProbability)
    {
        Debug.Log("Button 2 deals " + B2_damage + " damage.");
        B2_healAmount -= B2_damage;
         CharacterController[] chars = GameObject.FindObjectsOfType<CharacterController>();
        foreach(CharacterController cc in chars){
            if(cc.gameObject.tag == "Player"){
                cc.TakeDamage(B2_damage, "Damage");
            }

            cc.anim.SetBool("isIdle", false);
        }

    }
    else if (chance <= B2_damageProbability + B2_healProbability)
    {
        Debug.Log("Button 2 heals " + B2_healAmount + " health.");
        B2_healAmount += B2_healAmount;
         CharacterController[] chars = GameObject.FindObjectsOfType<CharacterController>();
        foreach(CharacterController cc in chars){
            if(cc.gameObject.tag == "Player"){
                cc.Heal(B2_healAmount);
            }

            cc.anim.SetBool("isIdle", false);
        }

    }
    else    
    {
        Debug.Log("Button 2 does nothing.");
        CharacterController[] chars = GameObject.FindObjectsOfType<CharacterController>();
        foreach(CharacterController cc in chars){
            cc.anim.SetBool("isIdle", false);
        }
        
    }
    StartCoroutine(FadeOutPanel());

}

// Coroutine to fade out the panel and all its contents
private IEnumerator FadeOutPanel()
{
          
    

    // Disable buttons before fade-out
    button1.interactable = false;
    button2.interactable = false;

    // Animate alpha to 0 over fadeDuration seconds
    float t = 0;
    while (t < fadeDuration)
    {
        float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
        Color panelColor = panel.GetComponent<Image>().color;
        panelColor.a = alpha;
        panel.GetComponent<Image>().color = panelColor;
        Color textColor = descriptionText.color;
        textColor.a = alpha;
        descriptionText.color = textColor;
        button1Text.color = textColor;
        button2Text.color = textColor;
        Color imageColor = image.color;
        imageColor.a = alpha;
        image.color = imageColor;
        Color titleColor = titleText.color;
        titleColor.a = alpha;
        titleText.color = titleColor;
        yield return null;
        t += Time.deltaTime;
    }
//  Set the spawn chance of room 0 to 100%
         //player.GetComponent<MoveRight>().Move();
         MoveRight[] move = GameObject.FindObjectsOfType<MoveRight>();
         foreach(MoveRight Move in move){
            Move.Move();
         }
    // Destroy this game object
    Destroy(gameObject);
}
 

    // Called when the script is enabled
    private void OnEnable()
    {
        Color titleColor = titleText.color;
        titleColor.a = 0;
      titleText.color = titleColor;
      titleText.text = titleInfo;

      


              // Assign the sprite to image2
image.sprite = sprite;

// Set the alpha to 0
Color image2Color = image.color;
image2Color.a = 0;
image.color = image2Color;


        // Set description text with color coding
        string description = "";
        foreach (ColorCodedTextField field in DescriptionFields)
        {
            description += "<color=#" + ColorUtility.ToHtmlStringRGB(field.color) + ">" + field.text + "</color>";
        }
        descriptionText.text = description;

        // Set button 1 text with color coding
        string button1Description = "";
        foreach (ColorCodedTextField field in Option_One_Fields)
        {
            button1Description += "<color=#" + ColorUtility.ToHtmlStringRGB(field.color) + ">" + field.text + "</color>";
        }
        button1Text.text = button1Description;

        // Set button 2 text with color coding
        string button2Description = "";
        foreach (ColorCodedTextField field in Option_Two_Fields)
        {
            button2Description += "<color=#" + ColorUtility.ToHtmlStringRGB(field.color) + ">" + field.text + "</color>";
        }
        button2Text.text = button2Description;

        // Fade in the panel
        StartCoroutine(FadeInPanel());
    }

    // Coroutine to fade in the panel
private IEnumerator FadeInPanel()
{
    // Set initial alpha to 0
    Color panelColor = panel.GetComponent<Image>().color;
    panelColor.a = 0;
    panel.GetComponent<Image>().color = panelColor;
    Color textColor = descriptionText.color;
    textColor.a = 0;
    descriptionText.color = textColor;
    button1Text.color = textColor;
    button2Text.color = textColor;
    image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    titleText.color = new Color(titleText.color.r, titleText.color.g, titleText.color.b, 0);

    // Animate alpha to 1 over fadeDuration seconds
    float t = 0;
    while (t < fadeDuration)
    {
        float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
        panelColor.a = alpha;
        panel.GetComponent<Image>().color = panelColor;
        textColor.a = alpha;
        descriptionText.color = textColor;
        button1Text.color = textColor;
        button2Text.color = textColor;
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        titleText.color = new Color(titleText.color.r, titleText.color.g, titleText.color.b, alpha);
        yield return null;
        t += Time.deltaTime;
    }

    // Set alpha to 1 after animation to ensure full visibility
    panelColor.a = 1;
    panel.GetComponent<Image>().color = panelColor;
    textColor.a = 1;
    descriptionText.color = textColor;
    button1Text.color = textColor;
    button2Text.color = textColor;
    image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    titleText.color = new Color(titleText.color.r, titleText.color.g, titleText.color.b, 1);

    // Enable buttons after fade-in
    button1.interactable = true;
    button2.interactable = true;
}


}
