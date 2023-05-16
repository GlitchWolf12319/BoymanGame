using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	
   public AudioClip buttonClick;
   public AudioSource speaker;
   public AudioSource AmbientSpeaker;
   public AudioSource MusicSpeaker;
   public AudioClip PoisonMusic;
   public AudioClip FireMusic;
   public AudioClip IceMusic;
   public AudioClip NonCombatMusic;
   public AudioClip CombatMusic;

   void Start(){
		PlayMusic("NonCombat");
   }


   public void ButtonClick(){
		speaker.clip = buttonClick;
		if(!speaker.isPlaying){
			speaker.Play();
		}
   }

   public void Attack(AudioClip attackSound){
		speaker.clip = attackSound;
		if(!speaker.isPlaying){
			speaker.Play();
		}
   }

   public void PlayAmbient(string name){
		if(AmbientSpeaker.isPlaying){
			AmbientSpeaker.Stop();
			
			CheckRoom(name);

			AmbientSpeaker.Play();


		}
		else if(!AmbientSpeaker.isPlaying){
			CheckRoom(name);
			AmbientSpeaker.Play();
		}
   }

   public void PlayMusic(string situation){
		MusicSpeaker.Stop();
		if(situation == "Combat"){
			if(!MusicSpeaker.isPlaying){
				MusicSpeaker.clip = CombatMusic;
				MusicSpeaker.Play();
			}
		}

		if(situation == "NonCombat"){
			if(!MusicSpeaker.isPlaying){
				MusicSpeaker.clip = NonCombatMusic;
				MusicSpeaker.Play();
			}
		}
   }

   public void CheckRoom(string name){
			if(name.Contains("Lava")){
				AmbientSpeaker.clip = FireMusic;
			}

			if(name.Contains("Ice")){
				AmbientSpeaker.clip = IceMusic;
			}

			if(name.Contains("Poison")){
				AmbientSpeaker.clip = PoisonMusic;
			}
   }

  

}
