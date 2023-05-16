using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public VideoPlayer video;
    public RawImage videoImage;
    public Image mainMenuSnap;
    public VideoClip introToMenu;

    // Start is called before the first frame update
    void Start()
    {
        // Deactivate the main menu snapshot image
        mainMenuSnap.gameObject.SetActive(false);

        // Start the coroutine to play the video in the menu
        StartCoroutine(VideoMenu(7, mainMenuSnap, introToMenu));
    }

    public IEnumerator VideoMenu(int duration, Image snap, VideoClip videoToPlay)
    {
        // Set the video clip to be played by the VideoPlayer
        video.clip = videoToPlay;

        // Activate the video image
        videoImage.gameObject.SetActive(true);

        // Play the video
        video.Play();

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Pause the video
        video.Pause();

        // Deactivate the video image
        videoImage.gameObject.SetActive(false);

        // Activate the main menu snapshot image
        snap.gameObject.SetActive(true);
    }

    public void PlayGame()
    {
        // Load the "NathanTest" scene
        SceneManager.LoadScene("NathanTest");
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
