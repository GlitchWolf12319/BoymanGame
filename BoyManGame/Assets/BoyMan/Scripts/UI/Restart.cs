using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : FindTargets
{
    public List<GameObject> targets;  // List of game objects representing the targets
    public GameObject buttons;  // Reference to the buttons game object
    public string activeScene;  // Name of the active scene

    public void Start()
    {
        targets = FindGoodChar();  // Find and assign the good characters to the targets list
    }

    public void removeTarget(GameObject target)
    {
        targets.Remove(target);  // Remove the specified target from the targets list

        if (targets.Count <= 0)
        {
            buttons.SetActive(true);  // Activate the buttons game object

            // Open two Google Forms URLs in the browser
            Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLScfXd34AmUOKV92MY-Zv9TzE0Gk-ajhF6PRlfsyR9xaDgSujA/viewform");
            Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdscRqhLkfzKXVuQLDUZZvqDxGOC0qMhoxUVywt9Hdo-8uynw/viewform?usp=sf_link");
        }
    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();  // Get the active scene
        activeScene = scene.name;  // Store the name of the active scene
        SceneManager.LoadScene(activeScene);  // Load the active scene
    }

    public void Quit()
    {
        Application.Quit();  // Quit the application
    }
}