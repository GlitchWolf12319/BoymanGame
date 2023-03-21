using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIdicator : MonoBehaviour
{
   public TMP_Text text;
   public float lifetime = 0.6f;
   public float minDist = 2f;
   public float maxDist = 3f;

   private Vector3 initialPosition;
   private Vector3 targetPosition;
   private float timer; 

   void Start(){
    //transform.LookAt(2 * transform.position - Camera.main.transform.position);

    float direction = Random.rotation.eulerAngles.z;
    initialPosition = transform.position;
    float dist = Random.Range(minDist, maxDist);
    targetPosition = initialPosition + (Quaternion.Euler(0, 0, direction) * new Vector3(dist, dist, 0));
    transform.localScale = Vector3.zero;
   }

   void Update(){

    timer += Time.deltaTime;

    float fraction = lifetime / 2;

    if(timer > lifetime){
        Destroy(gameObject);
    }
    else if(timer > fraction){
        text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));
    }

    transform.position = Vector3.Lerp(initialPosition, targetPosition, Mathf.Sin(timer / lifetime));
    transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));

   }

   public void SetDamageText(int damage){
        text.text = damage.ToString();
   }

   public void SetDamageColor(Color color){
    text.color = color;
   }
}
