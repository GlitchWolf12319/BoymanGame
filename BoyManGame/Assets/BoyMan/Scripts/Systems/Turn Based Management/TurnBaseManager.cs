using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnBaseManager : FindTargets
{
    public int turnCounter = -1; // Counter to keep track of the current turn
    public List<CharTurn> turns; // List of characters taking turns
    [SerializeField] private List<GameObject> enemiesInBattle; // List of enemy GameObjects in the battle
    [SerializeField] private List<GameObject> heroesInBattle; // List of player GameObjects in the battle
    [SerializeField] private List<GameObject> totalCharsInBattle; // List of all GameObjects in the battle
    [SerializeField] private GameObject RewardSystem; // Reference to the reward system GameObject
    public GameObject disabledUI; // Reference to the disabled UI GameObject
    public bool battleInProgress; // Flag indicating if a battle is in progress

    public bool APlayersTurn; // Flag indicating if it is currently the player's turn

    void Start()
    {
        disabledUI.transform.localScale = new Vector3(0, 0, 0); // Set the disabled UI scale to zero to hide it initially
    }

    public void ChangeTurn()
    {
        turnCounter++; // Increment the turn counter

        if (turnCounter > turns.Count - 1)
        {
            turnCounter = 0; // Reset the turn counter if it exceeds the number of turns
        }

        CharTurn currentTurn = turns[turnCounter]; // Get the current turn
        turns[turnCounter].StartCoroutine(turns[turnCounter].StartTurn()); // Start the turn coroutine for the current character

        if (turns[turnCounter].characterType == CharTurn.CharacterType.Player)
        {
            Debug.Log("Players Turn");
            APlayersTurn = true; // Set the flag to indicate it's the player's turn
        }
        else
        {
            Debug.Log("Enemies Turn");
            APlayersTurn = false; // Set the flag to indicate it's the enemy's turn
        }
    }

    public IEnumerator EnlargeDisabledUI()
    {
        yield return new WaitForSeconds(1);
        disabledUI.transform.localScale = new Vector3(1, 1, 1); // Enlarge the disabled UI after a delay
    }

    public void SetTurnOrder()
    {
        enemiesInBattle = FindEnemies(); // Find and store enemy GameObjects in the battle
        heroesInBattle = FindGoodChar(); // Find and store player GameObjects in the battle
        totalCharsInBattle.AddRange(FindEnemies()); // Add enemy GameObjects to the total characters in battle list
        totalCharsInBattle.AddRange(FindGoodChar()); // Add player GameObjects to the total characters in battle list

        for (int i = 0; i < heroesInBattle.Count + enemiesInBattle.Count; i++)
        {
            int randomChoice = Random.Range(0, totalCharsInBattle.Count);
            turns.Add(totalCharsInBattle[randomChoice].GetComponent<CharTurn>()); // Randomly assign turns to characters from the total characters list
            totalCharsInBattle.RemoveAt(randomChoice); // Remove the character from the total characters list after assigning the turn
        }

        battleInProgress = true; // Set the battle in progress flag to true
    }
    public void RemoveFromTurnOrder(GameObject DeadTarget, string damageType)
{
    if (!DeadTarget.GetComponent<CharacterController>().dead) // Check if the target is not already dead
    {
        Debug.Log("Removing " + DeadTarget.name);
        turns.RemoveAt(turns.IndexOf(DeadTarget.GetComponent<CharTurn>())); // Remove the character's turn from the turns list

        if (DeadTarget.GetComponent<CharTurn>().characterType == CharTurn.CharacterType.Player)
        {
            heroesInBattle.Remove(DeadTarget); // Remove the player character from the heroes in battle list
        }
        else if (DeadTarget.GetComponent<CharTurn>().characterType == CharTurn.CharacterType.Enemy)
        {
            enemiesInBattle.Remove(DeadTarget); // Remove the enemy character from the enemies in battle list
            Debug.Log(damageType);

            if (enemiesInBattle.Count > 0 && damageType != "NormalDamage") // Check if there are still enemies in battle and the damage type is not "NormalDamage"
            {
                ChangeTurn(); // Change the turn to the next character
            }
        }

        if (isBattleFinished() && heroesInBattle.Count > 0) // Check if the battle is finished and there are still player characters remaining
        {
            Debug.Log("Battle Finished");
            StartCoroutine(FinishBattle()); // Start the coroutine to finish the battle
            battleInProgress = false; // Set the battle in progress flag to false
        }
    }
}

public IEnumerator FinishBattle()
{
    turns.Clear(); // Clear the turns list
    GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayMusic("NonCombat"); // Play non-combat music
    yield return new WaitForSeconds(3);

    for (int i = 0; i < heroesInBattle.Count; i++)
    {
        for (int c = 0; c < heroesInBattle[i].GetComponent<NewDeckDrawing>().hand.Count; c++)
        {
            // Disable hovering, clicking, and other card interactions for player's hand cards
            heroesInBattle[i].GetComponent<NewDeckDrawing>().hand[c].GetComponent<Card>().disableHovering = true;
            heroesInBattle[i].GetComponent<NewDeckDrawing>().hand[c].GetComponent<Card>().canHover = false;
            heroesInBattle[i].GetComponent<NewDeckDrawing>().hand[c].GetComponent<Card>().canClick = false;
        }

        // Reset player character's properties and UI elements
        heroesInBattle[i].GetComponent<CharacterController>().health = heroesInBattle[i].GetComponent<CharacterController>().MaxHealth;
        heroesInBattle[i].GetComponent<CharTurn>().turnUI.SetActive(false);
        heroesInBattle[i].GetComponent<NewDeckDrawing>().ClearDeck();
        heroesInBattle[i].GetComponent<CharacterController>().guard = 0;
        heroesInBattle[i].GetComponent<CharacterController>().igniteStack = 0;
        heroesInBattle[i].GetComponent<CharacterController>().poisonStack = 0;
        heroesInBattle[i].GetComponent<CharacterController>().invisibleStack = 0;
        heroesInBattle[i].GetComponent<CharacterController>().Visible();
        heroesInBattle[i].GetComponent<CharTurn>().turnIcon.SetActive(false);
    }

    disabledUI.transform.localScale = new Vector3(0, 0, 0); // Hide the disabled UI

    yield return new WaitForSeconds(2.5f);
    GameObject RS = Instantiate(RewardSystem); // Instantiate the reward system GameObject
    RS.transform.SetAsFirstSibling(); // Set the reward system as the first sibling in the hierarchy
}


    public IEnumerator FleeBattle()
{
    turns.Clear(); // Clear the turns list

    // Destroy and remove enemy characters from the battle
    for (int e = 0; e < enemiesInBattle.Count; e++)
    {
        enemiesInBattle[e].transform.DOScale(new Vector3(0, 0, 0), 0.5f); // Shrink the enemy character
        yield return new WaitForSeconds(0.5f);
        Destroy(enemiesInBattle[e]); // Destroy the enemy character GameObject
    }

    // Reset properties and UI elements for player characters
    for (int h = 0; h < heroesInBattle.Count; h++)
    {
        heroesInBattle[h].GetComponent<CharacterController>().health = heroesInBattle[h].GetComponent<CharacterController>().MaxHealth;
        heroesInBattle[h].GetComponent<CharTurn>().turnUI.SetActive(false);
        heroesInBattle[h].GetComponent<NewDeckDrawing>().ClearDeck();
        heroesInBattle[h].GetComponent<CharacterController>().guard = 0;
        heroesInBattle[h].GetComponent<CharacterController>().igniteStack = 0;
        heroesInBattle[h].GetComponent<CharacterController>().poisonStack = 0;
        heroesInBattle[h].GetComponent<CharacterController>().invisibleStack = 0;
        heroesInBattle[h].GetComponent<CharacterController>().Visible();
        heroesInBattle[h].GetComponent<CharTurn>().turnIcon.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }

    heroesInBattle.Clear(); // Clear the list of player characters in battle
    enemiesInBattle.Clear(); // Clear the list of enemy characters in battle

    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
    {
        Movement[] move = FindObjectsOfType<Movement>();
        foreach (Movement Move in move)
        {
            Move.enabled = true;
        }
    }
}

public bool isBattleFinished()
{
    if (enemiesInBattle.Count <= 0 || heroesInBattle.Count <= 0) // Check if there are no enemies or no heroes remaining in battle
    {
        return true; // Battle is finished
    }
    else
    {
        return false; // Battle is not finished
    }
}
}
