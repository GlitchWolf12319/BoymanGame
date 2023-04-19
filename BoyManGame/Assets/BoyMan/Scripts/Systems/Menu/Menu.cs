using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public  VideoPlayer video;
    public RawImage videoImage;
    public Image mainMenuSnap;
    public VideoClip introToMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuSnap.gameObject.SetActive(false);
        StartCoroutine(VideoMenu(7, mainMenuSnap, introToMenu));
    }

    public IEnumerator VideoMenu(int duration, Image snap, VideoClip videoToPlay){
        video.clip = videoToPlay;
        videoImage.gameObject.SetActive(true);
        video.Play();
        yield return new WaitForSeconds(duration);
        video.Pause();
        videoImage.gameObject.SetActive(false);
        snap.gameObject.SetActive(true);
    }

    public void PlayGame(){
        SceneManager.LoadScene("NathanTest");
    }

    public void QuitGame(){
        Application.Quit();
    }

    

}
