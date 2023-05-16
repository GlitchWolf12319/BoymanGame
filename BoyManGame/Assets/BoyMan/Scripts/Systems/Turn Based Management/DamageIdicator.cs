using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIdicator : MonoBehaviour
{
    public TMP_Text text; // Reference to the TextMeshPro text component
    public float lifetime = 0.6f; // The lifetime duration of the damage text
    public float minDist = 2f; // The minimum distance for the target position
    public float maxDist = 3f; // The maximum distance for the target position

    private Vector3 initialPosition; // The initial position of the damage text
    private Vector3 targetPosition; // The target position the damage text moves towards
    private float timer; // The timer for tracking the lifetime of the damage text

    void Start()
    {
        // Generate a random direction in degrees
        float direction = Random.rotation.eulerAngles.z;

        initialPosition = transform.position; // Store the initial position of the damage text

        // Calculate a random target position within the specified range
        float dist = Random.Range(minDist, maxDist);
        targetPosition = initialPosition + (Quaternion.Euler(0, 0, direction) * new Vector3(dist, dist, 0));

        transform.localScale = Vector3.zero; // Set the initial scale of the damage text to zero
    }

    void Update()
    {
        timer += Time.deltaTime; // Update the timer with the elapsed time

        float fraction = lifetime / 2; // Calculate the fraction of the lifetime for fading the text

        if (timer > lifetime)
        {
            Destroy(gameObject); // If the lifetime is exceeded, destroy the damage text game object
        }
        else if (timer > fraction)
        {
            // Fade out the text color gradually as time progresses
            text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));
        }

        // Move the damage text from the initial position to the target position using a sine wave motion
        transform.position = Vector3.Lerp(initialPosition, targetPosition, Mathf.Sin(timer / lifetime));

        // Scale the damage text gradually from zero to one using a sine wave motion
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));
    }

    public void SetDamageText(int damage)
    {
        text.text = damage.ToString(); // Set the text of the damage text to display the provided damage value
    }

    public void SetDamageColor(Color color)
    {
        text.color = color; // Set the color of the damage text
    }
}