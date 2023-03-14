using UnityEngine;
using DG.Tweening;

public class UIPopIn : MonoBehaviour
{
    [SerializeField] private float popInDuration = 0.5f;
    [SerializeField] private Vector3 popInStrength = new Vector3(1f, 1f, 0f);
    [SerializeField] private float scaleBackDuration = 0.5f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOPunchScale(popInStrength, popInDuration).OnComplete(ScaleBack);
    }

    private void ScaleBack()
    {
        rectTransform.DOScale(Vector3.one, scaleBackDuration);
    }
}