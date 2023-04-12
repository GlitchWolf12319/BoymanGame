using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private AudioClip walking;
    public AudioClip damage;
    public AudioClip heal;

    void Start(){
        playerAudio.enabled = true;
    }

    void LateUpdate(){
        Move();
    }

    public void PlayOneShotAudio(AudioClip clip){
        playerAudio.enabled = true;
        playerAudio.PlayOneShot(clip);
    }

    void Move(){
        if(Input.GetKey(KeyCode.A)){
            transform.position += Vector3.left * speed * Time.deltaTime;
            spriteRend.flipX = true;
            anim.SetBool("isIdle", false);

            playerAudio.clip = walking;
            playerAudio.loop = true;
            playerAudio.enabled = true;
            if(!playerAudio.isPlaying){
                playerAudio.Play();
            }
            
            
        }
        

        if(Input.GetKey(KeyCode.D)){
            transform.position += Vector3.right * speed * Time.deltaTime;
            spriteRend.flipX = false;
            anim.SetBool("isIdle", false);

            playerAudio.clip = walking;
            playerAudio.loop = true;
            playerAudio.enabled = true;
            if(!playerAudio.isPlaying){
                playerAudio.Play();
            }
            
        }
        
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)){
            anim.SetBool("isIdle", true);

            playerAudio.clip = null;
            playerAudio.loop = false;
            playerAudio.enabled = false;
            playerAudio.Stop();
            
        }
    }

    public void StopEverything(){
        anim.SetBool("isIdle", true);

            playerAudio.clip = null;
            playerAudio.loop = false;
            playerAudio.enabled = false;
            playerAudio.Stop();
    }
}
