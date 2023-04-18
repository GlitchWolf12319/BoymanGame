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

        if(playerAudio != null){
            playerAudio.enabled = true;
        }
        
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

            if(spriteRend != null){
                spriteRend.flipX = true;
            }
            


            if(anim != null){
                anim.SetBool("isIdle", false);
            }
            

            if(playerAudio != null){
            playerAudio.clip = walking;
            playerAudio.loop = true;
            playerAudio.enabled = true;
            if(!playerAudio.isPlaying){
                playerAudio.Play();
            }
            }
            
            
        }
        

        if(Input.GetKey(KeyCode.D)){
            transform.position += Vector3.right * speed * Time.deltaTime;

            if(spriteRend != null){
                spriteRend.flipX = false;
            }

            if(anim != null){
                anim.SetBool("isIdle", false);
            }


            if(playerAudio != null){
                playerAudio.clip = walking;
                playerAudio.loop = true;
                playerAudio.enabled = true;
                if(!playerAudio.isPlaying){
                    playerAudio.Play();
                }
            }
            
            
        }
        
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)){

            if(anim != null){
                anim.SetBool("isIdle", true);
            }
            

            if(playerAudio != null){
                playerAudio.clip = null;
                playerAudio.loop = false;
                playerAudio.enabled = false;
                playerAudio.Stop();
            }
            
            
        }
    }

    public void StopEverything(){

            if(anim != null){
                anim.SetBool("isIdle", true);
            }
            

            if(playerAudio != null){
                playerAudio.clip = null;
                playerAudio.loop = false;
                playerAudio.enabled = false;
                playerAudio.Stop();
            }
    }
}
