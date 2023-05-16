using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icons : MonoBehaviour
{
    public Vector3 targetScale;  // The target scale to reach during the scaling animation
    public float duration = 1f;  // The duration of the scaling animation

    private Vector3 startScale;  // The initial scale of the object
    private float startTime;  // The start time of the animation
    private bool isGrowing = true;  // Flag indicating whether the object is currently growing
    private SpriteRenderer sr;  // Reference to the SpriteRenderer component

    void Start()
    {
        startScale = Vector3.zero;  // Set the initial scale to zero
        transform.localScale = startScale;  // Apply the initial scale to the object
        startTime = Time.time;  // Record the start time of the animation
        sr = this.GetComponent<SpriteRenderer>();  // Get the SpriteRenderer component of the object
    }

    private void Update()
    {
        if (isGrowing)
        {
            float timeRatio = (Time.time - startTime) / duration;  // Calculate the time ratio based on the current time and the duration

            // Interpolate the scale between the start scale and the target scale using the time ratio
            transform.localScale = Vector3.Lerp(startScale, targetScale, timeRatio);

            // Interpolate the alpha value of the sprite color between 1f and 0f using the time ratio
            Color spriteColor = sr.color;
            spriteColor.a = Mathf.Lerp(1f, 0f, timeRatio);
            sr.color = spriteColor;

            if (timeRatio >= 1f)
            {
                isGrowing = false;  // Set the flag to indicate that the object has finished growing
                Destroy(this.gameObject);  // Destroy the game object
            }
        }
    }
}