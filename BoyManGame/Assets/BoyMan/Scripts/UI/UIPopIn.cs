using UnityEngine;
using DG.Tweening;

public class UIPopIn : MonoBehaviour
{
    [SerializeField] private float popInDuration = 0.5f;  // The duration of the pop-in animation
    [SerializeField] private Vector3 popInStrength = new Vector3(1f, 1f, 0f);  // The strength of the pop-in animation
    [SerializeField] private float scaleBackDuration = 0.5f;  // The duration of the scale back animation

    private RectTransform rectTransform;  // Reference to the RectTransform component

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();  // Get the RectTransform component of the object
    }

    private void OnEnable()
    {
        rectTransform.localScale = Vector3.zero;  // Set the initial scale to zero
        rectTransform.DOPunchScale(popInStrength, popInDuration).OnComplete(ScaleBack);  // Apply the pop-in animation and call the ScaleBack method when complete
    }

    private void ScaleBack()
    {
        rectTransform.DOScale(Vector3.one, scaleBackDuration);  // Apply the scale back animation to restore the original scale
    }
}