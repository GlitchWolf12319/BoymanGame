using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

// Card class, inherits from FindTargets class
public class Card : FindTargets
{
    // Reference to the card template
    public CardTemplate card;

    // Flag to indicate if the card is selected
    public bool selected;

    // The original position of the card
    public Vector3 originalPosition;

    // The index of the card
    public int index;

    // The rotation of the card
    public Vector3 cardRotation;

    // List of cards close to this card
    public List<GameObject> CloseCards = new List<GameObject>();

    // List of cards far from this card
    public List<GameObject> FarCards = new List<GameObject>();

    // Flag to check if the card can select targets
    public bool canSelectTarget;

    // List of targets that can be attacked
    public List<GameObject> targets = new List<GameObject>();

    // Flag to indicate if the mouse is hovering over the card
    [SerializeField] public bool onHover = false;

    // Flag to check if the card is draggable
    public bool Drag = false;

    // Flag to check if the card is an arrow
    public bool Arrow = false;

    // Flag to check if the card can be hovered over
    public bool canHover;

    // Reference to the caster of the card
    [SerializeField] public GameObject caster;

    // Prefabs and colors used for visual effects
    public GameObject shieldIconPrefab;
    public GameObject arrow;
    public GameObject speechBubblePrefab;
    public Color canUseColor;
    public Color cantUseColor;

    // Flags and UI elements related to card usage
    public bool canSelect;
    public bool disableHovering;
    public GameObject descriptionPanel;
    public bool canClick;

    // Collect the targets for the card
    public void CollectTarget()
    {
        targets = FindEnemies();
    }

    // Select the target for the card
    public void SelectTarget()
    {
        if (Input.GetMouseButtonDown(0) && canClick)
        {
            Debug.Log("Click");
            // Create a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Perform the raycast and get the hit information
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if (hit.transform.name == targets[i].name)
                    {
                        CheckAbility(hit.transform.gameObject);
                    }
                }
            }
        }
    }

void CheckAbility(GameObject target)
{
    // Play a sound effect associated with the card
    if (card.cardSoundEffect != null)
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Attack(card.cardSoundEffect);
    }

    // Loop through the abilities of the card
    for (int i = 0; i < card.ability.Length; i++)
    {
        // Check each ability and perform the corresponding action

        // Deal damage to the target
        if (card.ability[i].dealDamage != null)
        {
            DealDamage(target, card.ability[i].dealDamage.damageAmmount);
        }

        // Deal damage to all targets in the party
        if (card.ability[i].dealPartyDamage != null)
        {
            DealPartyDamage(card.ability[i].dealPartyDamage.damageAmmount);
        }

        // Ignite the target
        if (card.ability[i].igniteEffect != null)
        {
            Ignite(target, card.ability[i].igniteEffect.IgniteStack, card.ability[i].igniteEffect.IgniteStack);
        }

        // Weaken the target
        if (card.ability[i].weaken != null)
        {
            Weaken(target, card.ability[i].weaken.WeakenStack);
        }

        // Poison the target
        if (card.ability[i].poisonEffect != null)
        {
            Poison(target, card.ability[i].poisonEffect.poisonStack, card.ability[i].poisonEffect.poisonStack);
        }

        // Give the target a guard
        if (card.ability[i].guard != null)
        {
            target = caster;
            GiveGuard(target, card.ability[i].guard.guardAmmount);
        }

        // Heal the target
        if (card.ability[i].heal != null)
        {
            target = caster;
            Heal(target, card.ability[i].heal.healAmmount);
        }

        // Heal all targets in the party
        if (card.ability[i].healParty != null)
        {
            HealParty(card.ability[i].healParty.healAmmount);
        }

        // Apply the chilled effect to the target
        if (card.ability[i].chilled != null)
        {
            Chilled(target, card.ability[i].chilled.chilledStack);
        }

        // Make the target invisible
        if (card.ability[i].invisible != null)
        {
            target = caster;
            Invisible(target, card.ability[i].invisible.invisibleStack);
        }

        // Retreat from the current target to a random target
        if (card.ability[i].retreat != null)
        {
            List<GameObject> possibleTargets = FindGoodChar();
            if (possibleTargets != null)
            {
                GameObject RetreatTarget = null;
                while (RetreatTarget == null)
                {
                    int randomChoice = Random.Range(0, possibleTargets.Count);
                    if (possibleTargets[randomChoice].transform.name != this.transform.name)
                    {
                        RetreatTarget = possibleTargets[randomChoice];
                    }
                }

                if (RetreatTarget != null)
                {
                    Retreat(target, RetreatTarget);
                    possibleTargets.Clear();
                }
            }
        }

        // Ignite all enemies
        if (card.ability[i].igniteParty != null)
        {
            IgniteAllEnemies(card.ability[i].igniteParty.IgniteStack, card.ability[i].igniteParty.IgniteStack);
        }

        //clears targets
        targets.Clear();
    }

        // Check if the target's tag is not "Player" or if the card's ability length is greater than 1
if (target.tag != "Player" || card.ability.Length > 1)
{
    // Start a coroutine to perform a camera attack on the target
    StartCoroutine(CameraAttack(target));
    
    // Start a coroutine to indicate that the card is in use
    StartCoroutine(CardInUse());
}

// Check if the target's tag is "Player"
if (target.tag == "Player")
{
    // Start a coroutine to indicate that the card is in use
    StartCoroutine(CardInUse());
}

// Reduce the caster's action points by the cost of the card
caster.GetComponent<CharTurn>().ActionPoints -= card.APCost;

// Start a coroutine to move the card to the discard pile
caster.GetComponent<NewDeckDrawing>().StartCoroutine(caster.GetComponent<NewDeckDrawing>().MoveToDiscardPile(this.gameObject));

// Disable various flags and variables related to card selection and hovering
canSelectTarget = false;
selected = false;
canHover = false;
onHover = false;
Arrow = false;

// Loop through each card in the caster's hand
for (int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++)
{
    // Enable hovering and selection for the cards in the hand
    caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().disableHovering = false;
    caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().canSelect = true;
}
        
    }

    // Coroutine that handles the card being used
public IEnumerator CardInUse()
{
    // Set the card as in use
    CardInUse(true);

    // Wait for 3 seconds
    yield return new WaitForSeconds(3);

    // Set the card as not in use
    CardInUse(false);
}

// Method that handles the card being used or not used
public void CardInUse(bool inUse)
{
    // Find the TurnBaseManager instance in the scene
    TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();

    // Loop through each card in the caster's hand
    for (int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++)
    {
        // Check if the current card is not the same as the one being used
        if (caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().index != index)
        {
            // If the card is being used
            if (inUse)
            {
                // Move the card downwards (presumably off the screen) over a duration of 0.5 seconds
                caster.GetComponent<NewDeckDrawing>().hand[i].transform.DOMoveY(caster.GetComponent<NewDeckDrawing>().hand[i].transform.position.y - 5000, 0.5f);
                
                // Deactivate the turn UI for the caster
                caster.GetComponent<CharTurn>().turnUI.SetActive(false);
            }

            // If the card is not being used
            if (!inUse)
            {
                // Move the card upwards (presumably back onto the screen) over a duration of 0.5 seconds
                caster.GetComponent<NewDeckDrawing>().hand[i].transform.DOMoveY(caster.GetComponent<NewDeckDrawing>().hand[i].transform.position.y + 5000, 0.5f);

                // Check if the battle is not finished
                if (tbm.isBattleFinished() == false)
                {
                    // Check the current action points against the card
                    CheckCurrentAPAgainstCard(caster.GetComponent<NewDeckDrawing>().hand[i]);

                    // Activate the turn UI for the caster
                    caster.GetComponent<CharTurn>().turnUI.SetActive(true);
                }
            }
        }
    }
}

    //assign caster
    public void AssingCaster(){
        Transform CardsParent = this.transform.parent;
        caster = CardsParent.parent.gameObject;
    }

    void Update(){

        // Check if the target can be selected
    if(canSelectTarget){
        SelectTarget();
    }
    
    // Check for left mouse button click, hover, and selection conditions
    if(Input.GetMouseButtonDown(0) && onHover && !selected && canSelect && !selected){
        // Check if the caster has enough action points to use the card
        if(caster.GetComponent<CharTurn>().ActionPoints >= card.APCost){

            // Check the attack method of the card
            if(card.attackMethod == CardTemplate.AttackMethod.Drag){
                // Enable dragging mode
                if(caster.GetComponent<CharTurn>().ActionPoints >= card.APCost){
                    Drag = true;

                    // Disable card selection and hovering for cards in the player's hand
                    for(int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++){
                        caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().canSelect = false;
                        caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().disableHovering = true;
                    }
                }
                else{
                    StartCoroutine(cantUseCard()); // Start a coroutine to handle inability to use the card
                }
            }

            if(card.attackMethod == CardTemplate.AttackMethod.Arrow){
                // Enable arrow mode
                if(caster.GetComponent<CharTurn>().ActionPoints >= card.APCost){
                    Arrow = true;

                    // Disable card selection and hovering for cards in the player's hand
                    for(int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++){
                        caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().canSelect = false;
                        caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().disableHovering = true;
                    }
                }
                else{
                    StartCoroutine(cantUseCard()); // Start a coroutine to handle inability to use the card
                }
                
            }

            selected = true;
            Debug.Log("selected true");
            canSelectTarget = true;
            CollectTarget();
        }
        else{
            StartCoroutine(cantUseCard()); // Start a coroutine to handle inability to use the card
        }
    }
        
    // Check for left mouse button release and selected state
    if(Input.GetMouseButtonUp(0) && selected){
        // Check the attack method of the card
        if(card.attackMethod == CardTemplate.AttackMethod.Drag){
            selected = false;
            Debug.Log("selected false");
            Drag = false;
            CheckIfCardIsPlayed();

            // Enable card selection and hovering for cards in the player's hand
            for(int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++){
                caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().canSelect = true;
                caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().disableHovering = false;
            }
        }
    }

// Check if the right mouse button is pressed down and the mouse is hovering over an object
if (Input.GetMouseButtonDown(1) && onHover)
{
    selected = false; // Set the 'selected' variable to false
    Debug.Log("selected false"); // Output a debug message

    canSelectTarget = false; // Set the 'canSelectTarget' variable to false

    // Check the attack method of the card
    if (card.attackMethod == CardTemplate.AttackMethod.Drag)
    {
        Drag = false; // Set the 'Drag' variable to false
        transform.DOMove(originalPosition, 0.5f); // Move the transform to its original position using a DOTween animation
    }

    if (card.attackMethod == CardTemplate.AttackMethod.Arrow)
    {
        Arrow = false; // Set the 'Arrow' variable to false
    }

    // Enable selection and hovering for all cards in the hand
    for (int i = 0; i < caster.GetComponent<NewDeckDrawing>().hand.Count; i++)
    {
        caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().canSelect = true;
        caster.GetComponent<NewDeckDrawing>().hand[i].GetComponent<Card>().disableHovering = false;
    }
}

// If the 'Drag' variable is true, move the card based on the mouse position
if (Drag)
{
    MoveCardByMouse();
}

// If the 'Arrow' variable is true, enable the arrow game object; otherwise, disable it
if (Arrow)
{
    arrow.SetActive(true);
}
else
{
    arrow.SetActive(false);
}
        
    }

    
// Coroutine that handles the animation and logic when a card cannot be used
IEnumerator cantUseCard()
{
    // Define the positions for moving the card
    Vector3 upPos = new Vector3(0, originalPosition.y + 200, 0);
    Vector3 downPos = new Vector3(0, upPos.y - 100, 0);

    // Move the card up
    transform.DOMoveY(upPos.y, 0.5f);

    // Wait for 0.2 seconds
    yield return new WaitForSeconds(0.2f);

    // Move the card down
    transform.DOMoveY(downPos.y, 0.5f);

    // Find the game object with the tag "NoAPBubble"
    GameObject noAPBubble = GameObject.FindGameObjectWithTag("NoAPBubble");

    // If no object with the tag is found
    if (noAPBubble == null)
    {
        // Instantiate a speech bubble prefab at the caster's position
        GameObject bubble = Instantiate(speechBubblePrefab, caster.transform.position, Quaternion.identity);

        // Adjust the position of the bubble slightly
        bubble.transform.position = new Vector3(bubble.transform.position.x + 1, bubble.transform.position.y + 2, bubble.transform.position.z);

        // Destroy the bubble after 5 seconds
        Destroy(bubble, 5);
    }
}

public void CheckCurrentAPAgainstCard(GameObject cards){
    // Check if the AP cost of the card is greater than the remaining action points of the caster
    if(cards.GetComponent<Card>().card.APCost > caster.GetComponent<CharTurn>().ActionPoints){
        // If the card cannot be used due to insufficient action points, change the color of the outline and APCost text to "cantUseColor"
        foreach(Transform children in cards.transform){
            if(children.name.Contains("Outline")){
                Image ImageOutline = children.GetComponent<Image>();
                ImageOutline.color = cantUseColor;
            }
        }
        cards.GetComponent<CardRender>().APCost.color = cantUseColor;
    }
    // Check if the AP cost of the card is less than or equal to the remaining action points of the caster
    else if(cards.GetComponent<Card>().card.APCost <= caster.GetComponent<CharTurn>().ActionPoints){
        // If the card can be used, change the color of the outline to "canUseColor" and set the APCost text color to white
        foreach(Transform children in cards.transform){
            if(children.name.Contains("Outline")){
                Image ImageOutline = children.GetComponent<Image>();
                ImageOutline.color = canUseColor;
            }
        }
        cards.GetComponent<CardRender>().APCost.color = Color.white;
    }
}

// This function checks if a card has been played by comparing its current position with a target Y position
void CheckIfCardIsPlayed(){
    // Calculate the target Y position by adding 500 units to the original position's Y coordinate
    float YPos = originalPosition.y + 500;

    // Check if the card's current Y position is greater than the target Y position
    if(transform.position.y > YPos){
        // Call the CheckAbility function, passing in the current game object as a parameter
        CheckAbility(this.gameObject);

        // Set the 'selected', 'Drag', and 'onHover' flags to false
        selected = false;
        Drag = false;
        onHover = false;
    }
    else{
        // If the card's Y position is not greater than the target Y position, move the card back to its original position using DOTween
        transform.DOMove(originalPosition, 0.5f);

        // Disable the ability to select a target and clear the list of targets
        canSelectTarget = false;
        targets.Clear();
    }
}

// This function moves the card's position based on the current mouse position
void MoveCardByMouse(){
    // Set the card's position to the current mouse position
    transform.position = Input.mousePosition;
}

    public void Hover(){
        if(!Drag && canHover && !Arrow && !disableHovering){
// Set the variable 'canClick' to true, indicating that clicking is allowed
canClick = true;

// Activate the 'descriptionPanel' game object, making it visible
descriptionPanel.SetActive(true);

// Get a reference to the 'NewDeckDrawing' component attached to the 'caster' game object
NewDeckDrawing dd = caster.GetComponent<NewDeckDrawing>();

// Set the variable 'onHover' to true, indicating that the mouse is hovering over the object
onHover = true;

// Store the current rotation of the object in 'cardRotation' variable
cardRotation = transform.eulerAngles;

// Animate the object's scale to (1, 1, 1) over a duration of 0.5 seconds
transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);

// Animate the object's rotation to (0, 0, 0) over a duration of 0.5 seconds
transform.DORotate(new Vector3(0, 0, 0), 0.5f);

// If the 'index' variable is 0 or 4
if (index == 0 || index == 4)
{
    // Animate the object's vertical position upwards by 210 units over a duration of 0.5 seconds
    transform.DOMoveY(transform.position.y + 210, 0.5f);
}

// If the 'index' variable is 1 or 3
if (index == 1 || index == 3)
{
    // Animate the object's vertical position upwards by 120 units over a duration of 0.5 seconds
    transform.DOMoveY(transform.position.y + 120, 0.5f);
}

// If the 'index' variable is 2
if (index == 2)
{
    // Animate the object's vertical position upwards by 100 units over a duration of 0.5 seconds
    transform.DOMoveY(transform.position.y + 100, 0.5f);
}

        
        
        
      int LastIndex = dd.hand.Count - 1; // Get the index of the last element in the "hand" list

for (int i = 0; i < dd.hand.Count; i++) // Iterate through each element in the "hand" list
{
    CloseCards.Add(dd.hand[i]); // Add the current element to the "CloseCards" list

    // Check if the index of the current element is not within the range of (index - 1) to (index + 1)
    if (dd.hand[i].GetComponent<Card>().index < index - 1 || dd.hand[i].GetComponent<Card>().index > index + 1)
    {
        CloseCards.Remove(dd.hand[i]); // Remove the current element from the "CloseCards" list
        FarCards.Add(dd.hand[i]); // Add the current element to the "FarCards" list
        CloseCards.Remove(this.gameObject); // Remove this game object from the "CloseCards" list
    }
}

     // Iterate through each element in the "CloseCards" list
for (int i = 0; i < CloseCards.Count; i++)
{
    float[] Closeoffset = { CloseCards[i].GetComponent<Card>().originalPosition.x + 120, CloseCards[i].GetComponent<Card>().originalPosition.x - 120 };

    // Check if the index of the current element is less than the stored index
    if (index < CloseCards[i].GetComponent<Card>().index)
    {
        // Move the current element along the X-axis using DOTween to the positive offset value in 0.5 seconds
        CloseCards[i].transform.DOMoveX(Closeoffset[0], 0.5f);
    }
    // Check if the index of the current element is greater than the stored index
    else if (index > CloseCards[i].GetComponent<Card>().index)
    {
        // Move the current element along the X-axis using DOTween to the negative offset value in 0.5 seconds
        CloseCards[i].transform.DOMoveX(Closeoffset[1], 0.5f);
    }
}

// Iterate through each element in the "FarCards" list
for (int i = 0; i < FarCards.Count; i++)
{
    float[] Faroffset = { FarCards[i].GetComponent<Card>().originalPosition.x + 100, FarCards[i].GetComponent<Card>().originalPosition.x - 100 };

    // Check if the index of the current element is less than the stored index
    if (index < FarCards[i].GetComponent<Card>().index)
    {
        // Move the current element along the X-axis using DOTween to the positive offset value in 0.5 seconds
        FarCards[i].transform.DOMoveX(Faroffset[0], 0.5f);
    }
    // Check if the index of the current element is greater than the stored index
    else if (index > FarCards[i].GetComponent<Card>().index)
    {
        // Move the current element along the X-axis using DOTween to the negative offset value in 0.5 seconds
        FarCards[i].transform.DOMoveX(Faroffset[1], 0.5f);
    }
}

        }
    
    }

  public void NoHover()
{
    if (!Drag && canHover && !Arrow && !disableHovering)
    {
        canClick = false; // Disable click functionality
        descriptionPanel.SetActive(false); // Hide the description panel
        onHover = false; // Set the onHover flag to false
        transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.5f); // Scale down the transform to a smaller size over 0.5 seconds
        transform.DOMove(originalPosition, 0.5f); // Move the transform to its original position over 0.5 seconds
        transform.DORotate(cardRotation, 0.5f); // Rotate the transform to the specified cardRotation over 0.5 seconds
        CloseCards.Clear(); // Clear the CloseCards list
        FarCards.Clear(); // Clear the FarCards list

        NewDeckDrawing dd = caster.GetComponent<NewDeckDrawing>(); // Get the NewDeckDrawing component from the caster game object

        // Iterate through each element in the "hand" list of dd
        for (int i = 0; i < dd.hand.Count; i++)
        {
            // Move the current element's transform to its original position over 0.5 seconds
            dd.hand[i].transform.DOMove(dd.hand[i].GetComponent<Card>().originalPosition, 0.5f);
        }
    }
}


  void Pull(GameObject target, GameObject target2)
{
    Vector3 target1Pos = target.transform.position; // Store the position of the first target game object

    // Move the first target game object along the X-axis to the X-position of the second target game object over 0.5 seconds
    target.transform.DOMoveX(target2.transform.position.x, 0.5f);

    // Move the second target game object along the X-axis to the X-position of the first target game object over 0.5 seconds
    target2.transform.DOMoveX(target1Pos.x, 0.5f);
}

void Push(GameObject target, GameObject target2)
{
    Vector3 target1Pos = target.transform.position; // Store the position of the first target game object

    // Move the first target game object along the X-axis to the X-position of the second target game object over 0.5 seconds
    target.transform.DOMoveX(target2.transform.position.x, 0.5f);

    // Move the second target game object along the X-axis to the X-position of the first target game object over 0.5 seconds
    target2.transform.DOMoveX(target1Pos.x, 0.5f);
}

void Retreat(GameObject target, GameObject target2)
{
    Vector3 target1Pos = target.transform.position; // Store the position of the first target game object

    // Move the first target game object along the X-axis to the X-position of the second target game object over 0.5 seconds
    target.transform.DOMoveX(target2.transform.position.x, 0.5f);

    // Move the second target game object along the X-axis to the X-position of the first target game object over 0.5 seconds
    target2.transform.DOMoveX(target1Pos.x, 0.5f);
}

void GiveGuard(GameObject target, int amount)
{
    // Instantiate a shield icon prefab at the current transform position
    GameObject shield = Instantiate(shieldIconPrefab, transform.position, Quaternion.identity);

    // Get the CharacterController component from the caster game object and call the GainGuard method with the specified amount
    caster.GetComponent<CharacterController>().GainGuard(amount);
}

void Heal(GameObject target, int amount)
{
    int healAmount = amount; // Store the specified amount in a local variable

    // Get the CharacterController component from the target game object and call the Heal method with the healAmount
    target.GetComponent<CharacterController>().Heal(healAmount);
}

  public void DealDamage(GameObject target, int amount)
{
    // Find the TurnBaseManager instance in the scene
    TurnBaseManager tbm = FindObjectOfType<TurnBaseManager>();

    // Check if the current turn's character has the "weaken" effect
    if (tbm.turns[tbm.turnCounter].GetComponent<CharacterController>().hasWeaken())
    {
        Debug.Log("Has Weaken");
        
        // Calculate 25% of the specified amount
        float quarterAmount = amount * 0.25f;

        // Subtract the quarter amount from the specified amount to get the new damage value
        float newDamage = amount - quarterAmount;

        // Round the new damage value to the nearest integer
        int intDamage = Mathf.RoundToInt(newDamage);

        Debug.Log("Weaken Damage " + intDamage);

        // Call the TakeDamage method of the target's CharacterController, passing the new damage value and the damage type as arguments
        target.GetComponent<CharacterController>().TakeDamage(intDamage, "NormalDamage");
    }
    else
    {
        // Call the TakeDamage method of the target's CharacterController, passing the specified amount and the damage type as arguments
        target.GetComponent<CharacterController>().TakeDamage(amount, "NormalDamage");
    }
}
    void Ignite(GameObject target, int ammount, int stack){

         // Set the ignite amount and stack on the target's CharacterController
            target.GetComponent<CharacterController>().igniteAmmount = ammount;
            target.GetComponent<CharacterController>().igniteStack += stack;
    }

    void Weaken(GameObject target, int stack){
            // Increase the weaken stack on the target's CharacterController

            target.GetComponent<CharacterController>().WeakenStack += stack;
    }

    void Poison(GameObject target, int ammount, int stack){

    // Set the poison amount and stack on the target's CharacterController

            target.GetComponent<CharacterController>().poisonAmmount = ammount;
            target.GetComponent<CharacterController>().poisonStack += stack;
    }

    void Chilled(GameObject target, int stack){
            // Increase the chilled stack on the target's CharacterController

        target.GetComponent<CharacterController>().chilledStack += stack;
    }

    void Invisible(GameObject target, int stack){
            // Set the invisible stack on the target's CharacterController and make the target invisible

        target.GetComponent<CharacterController>().invisibleStack = stack;
        target.GetComponent<CharacterController>().Invisible();
    }

    void DealPartyDamage(int ammount){
            // Find all enemy targets
            List<GameObject> targets = FindEnemies();
    // Deal damage to each enemy target
            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().TakeDamage(ammount, "NormalDamage");
            }
    }
   
    void IgniteAllEnemies(int ammount, int stack){
            // Find all enemy targets
            List<GameObject> targets = FindEnemies();
                // Apply ignite effect to each enemy target
            for(int i = 0; i < targets.Count; i++){
                targets[i].GetComponent<CharacterController>().igniteAmmount = ammount;
                targets[i].GetComponent<CharacterController>().igniteStack += stack;
            }
    }

   void HealParty(int ammount){
            Debug.Log("Healing Party");
                // Find all friendly targets
            List<GameObject> targets = FindGoodChar();
            
    // Heal each friendly target
            for(int i = 0; i < targets.Count; i++){
                Debug.Log(targets[i].name);
                targets[i].GetComponent<CharacterController>().Heal(ammount);
            }
   }

public IEnumerator CameraAttack(GameObject targetPos)
{
    // Store the original position of the caster
    Vector3 ogPos = caster.transform.position;
    Debug.Log(ogPos);
    
    // Get the main camera and enable zooming in
    Camera cam = Camera.main;
    cam.GetComponent<CameraZoom>().shouldZoomIn = true;
    
    // Move the caster to the target position using a Tween animation
    caster.transform.DOMoveX(cam.GetComponent<CameraZoom>().target.position.x, 1);
    
    // Wait for 1 second
    yield return new WaitForSeconds(1);
    
    // Check if there is an attack effect assigned to the card
    if (card.AttackEffect != null)
    {
        // Instantiate the attack effect at the target position
        GameObject effect = Instantiate(card.AttackEffect, targetPos.transform.position, Quaternion.identity);
        
        // Destroy the effect after 1.6 seconds
        Destroy(effect, 1.6f);
    }
    
    // Wait for 2 seconds
    yield return new WaitForSeconds(2);
    
    // Disable zooming in the camera
    cam.GetComponent<CameraZoom>().shouldZoomIn = false;
    
    // Move the caster back to its original position using a Tween animation
    caster.transform.DOMove(ogPos, 1);
    
    // Destroy the current game object immediately
    DestroyImmediate(this.gameObject, true);
}
}
