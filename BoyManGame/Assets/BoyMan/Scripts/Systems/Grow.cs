using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grow : MonoBehaviour
{
    [Header("Settings for Growing")]
    public Vector3 targetScale;
    public float duration = 1f;

    private Vector3 startScale;
    private float startTime;
    private bool isGrowing = true;


    private void Start()
    {
        startScale = Vector3.zero;
        transform.localScale = startScale;
        startTime = Time.time;
    }

    private void Update()
    {
        if (isGrowing)
        {
            float timeRatio = (Time.time - startTime) / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, timeRatio);

            if (timeRatio >= 1f)
            {
                isGrowing = false;
                Destroy(this.gameObject);
            }
        }
    }
}