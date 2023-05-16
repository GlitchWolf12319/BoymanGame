using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   // Audio clips
   public AudioClip buttonClick;
   public AudioClip PoisonMusic;
   public AudioClip FireMusic;
   public AudioClip IceMusic;
   public AudioClip NonCombatMusic;
   public AudioClip CombatMusic;

   // Audio sources
   public AudioSource speaker;
   public AudioSource AmbientSpeaker;
   public AudioSource MusicSpeaker;

   void Start(){
      // Play non-combat music at the start
      PlayMusic("NonCombat");
   }

   public void ButtonClick(){
      // Set the button click sound and play it if it's not already playing
      speaker.clip = buttonClick;
      if(!speaker.isPlaying){
         speaker.Play();
      }
   }

   public void Attack(AudioClip attackSound){
      // Set the attack sound and play it if it's not already playing
      speaker.clip = attackSound;
      if(!speaker.isPlaying){
         speaker.Play();
      }
   }

   public void PlayAmbient(string name){
      if(AmbientSpeaker.isPlaying){
         // If an ambient sound is already playing, stop it
         AmbientSpeaker.Stop();
         
         // Check the room and set the appropriate ambient sound
         CheckRoom(name);

         // Play the new ambient sound
         AmbientSpeaker.Play();
      }
      else if(!AmbientSpeaker.isPlaying){
         // If no ambient sound is playing, check the room and play the appropriate sound
         CheckRoom(name);
         AmbientSpeaker.Play();
      }
   }

   public void PlayMusic(string situation){
      // Stop the current music
      MusicSpeaker.Stop();
      
      // Check the situation and play the appropriate music
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
      // Check the room name and set the appropriate ambient sound
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
