using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icons : MonoBehaviour
{
    public Vector3 targetScale;
    public float duration = 1f;

    private Vector3 startScale;
    private float startTime;
    private bool isGrowing = true;
    private SpriteRenderer sr;

    void Start(){
        startScale = Vector3.zero;
        transform.localScale = startScale;
        startTime = Time.time;
        sr = this.GetComponent<SpriteRenderer>();
    }

    private void Update()
{
    if (isGrowing)
    {
        float timeRatio = (Time.time - startTime) / duration;
        transform.localScale = Vector3.Lerp(startScale, targetScale, timeRatio);
        
        Color spriteColor = sr.color;
        spriteColor.a = Mathf.Lerp(1f, 0f, timeRatio);
        sr.color = spriteColor;

        if (timeRatio >= 1f)
        {
            isGrowing = false;
            Destroy(this.gameObject); 
        }
    }
}


}
