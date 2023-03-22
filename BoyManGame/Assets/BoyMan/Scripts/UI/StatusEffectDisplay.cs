using UnityEngine;
using UnityEngine.UI;

public class StatusEffectDisplay : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectPrefab;
    [SerializeField] private Sprite[] statusEffectSprites;
    [SerializeField] private CharacterController characterController;

    private GameObject[] activeStatusEffects;

    void Awake()
    {
        activeStatusEffects = new GameObject[statusEffectSprites.Length];
        // Using this to test stacking images to the right
        // DisplayStatusEffect(1,"12");
    }
 void Update(){

        //Deffence Icon
      if(characterController.guard > 0){
        DisplayStatusEffect(0,characterController.guard.ToString());
      }
      else{
        HideStatusEffect(0);
      }

      if(characterController.igniteStack > 0){
        DisplayStatusEffect(1,characterController.igniteStack.ToString());
      }
      else{
        HideStatusEffect(1);
      }

      if(characterController.poisonStack > 0){
        DisplayStatusEffect(2,characterController.poisonStack.ToString());
      }
      else{
        HideStatusEffect(2);
      }


      
      



    }

 // Call this method with the index of the sprite you want to display and the text you want to display with it
    public void DisplayStatusEffect(int index, string text)
    {
        if (index >= 0 && index < statusEffectSprites.Length)
        {
            // Check if the status effect is already active
            if (activeStatusEffects[index] != null)
            {
                // If so, update the text of the existing image's text component
                Text statusEffectText = activeStatusEffects[index].GetComponentInChildren<Text>();
                if (statusEffectText != null)
                {
                    statusEffectText.text = text;
                }
            }
            else
            {
                // Otherwise, spawn a new image at the start position
                GameObject newStatusEffect = Instantiate(statusEffectPrefab, transform);
                Image statusEffectImage = newStatusEffect.GetComponent<Image>();
                statusEffectImage.sprite = statusEffectSprites[index];

                // Set the text of the new image's text component
                Text statusEffectText = newStatusEffect.GetComponentInChildren<Text>();
                if (statusEffectText != null)
                {
                    statusEffectText.text = text;
                }

                // Position the new status effect game object at the right of the previous one
                for (int i = 0; i < activeStatusEffects.Length; i++)
                {
                    if (activeStatusEffects[i] != null)
                    {
                        RectTransform prevRectTransform = activeStatusEffects[i].GetComponent<RectTransform>();
                        RectTransform newRectTransform = newStatusEffect.GetComponent<RectTransform>();
                        Vector2 newPos = prevRectTransform.anchoredPosition + new Vector2(36f, 0);// Change the 36f to make the stack space between icons bigger
                        newRectTransform.anchoredPosition = newPos;
                        break;
                    }
                }

                activeStatusEffects[index] = newStatusEffect;
            }
        }
        else
        {
            Debug.LogWarning($"Invalid status effect index {index}");
        }
    }
    // Call this method with the index of the sprite you want to hide
    public void HideStatusEffect(int index)
    {
        if (index >= 0 && index < statusEffectSprites.Length)
        {
            if (activeStatusEffects[index] != null)
            {
                Destroy(activeStatusEffects[index]);
                activeStatusEffects[index] = null;
            }
        }
        else
        {
            Debug.LogWarning($"Invalid status effect index {index}");
        }
    }

    // Call this method to hide all status effect images
    public void HideAllStatusEffects()
    {
        foreach (GameObject statusEffect in activeStatusEffects)
        {
            if (statusEffect != null)
            {
                Destroy(statusEffect);
            }
        }
        activeStatusEffects = new GameObject[statusEffectSprites.Length];
    }
}