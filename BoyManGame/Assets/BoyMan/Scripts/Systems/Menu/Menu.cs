using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Menu : MonoBehaviour
{

    public  VideoPlayer video;
    public RawImage videoImage;
    public Image mainMenuSnap;
    public Image settingsSnap;
    public VideoClip introToMenu;
    public VideoClip menuToSettings;
    public VideoClip settingsToMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuSnap.gameObject.SetActive(false);
        settingsSnap.gameObject.SetActive(false);
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

    public void Settings(){
        mainMenuSnap.gameObject.SetActive(false);
        settingsSnap.gameObject.SetActive(false);
        StartCoroutine(VideoMenu(4, settingsSnap, menuToSettings));
    }

    public void backToMenu(){
        mainMenuSnap.gameObject.SetActive(false);
        settingsSnap.gameObject.SetActive(false);
        StartCoroutine(VideoMenu(4, mainMenuSnap, settingsToMenu));
    }

    

}
