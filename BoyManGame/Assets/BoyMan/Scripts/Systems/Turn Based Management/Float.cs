using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Float : MonoBehaviour
{
    public float duration = 1f; // Duration of the float effect
    public float distance = 0.5f; // Distance the object should float up and down
    public Ease easeType = Ease.InOutSine; // Type of ease used for the float effect

    private void Start()
    {
        transform.DOMoveY(transform.position.y + distance, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo); // Loop the float effect indefinitely
    }
}
