using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minOrthoSize = 1f;
    [SerializeField] private float maxOrthoSize = 20f;
    [SerializeField] public bool shouldZoomIn = false;
    [SerializeField] private float defaultOrthoSize = 10f;

    private Camera cam;
    private Tween zoomTween;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        defaultOrthoSize = cam.orthographicSize;
    }
    
    void Update()
    {
        if (target != null)
        {
            Zoom();
        }
    }

    private void Zoom()
    {
        if (zoomTween != null && zoomTween.IsActive() && zoomTween.IsPlaying())
        {
            return;
        }

        float newSize;

        if (shouldZoomIn)
        {
            newSize = Mathf.Clamp(cam.orthographicSize - zoomSpeed * Time.deltaTime, minOrthoSize, maxOrthoSize);
        }
        else
        {
            newSize = defaultOrthoSize;
        }

        zoomTween = cam.DOOrthoSize(newSize, Mathf.Abs(cam.orthographicSize - newSize) / zoomSpeed)
            .SetEase(Ease.InOutSine);
    }
}