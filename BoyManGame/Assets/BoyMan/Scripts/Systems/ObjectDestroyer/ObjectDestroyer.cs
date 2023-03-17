using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [SerializeField] private float duration = 2.0f;

    void Start()
    {
        // Invoke the DestroyObject method after 'duration' seconds
        Invoke("DestroyObject", duration);
    }

    void DestroyObject()
    {
        // Destroy the object this script is attached to
        Destroy(gameObject);
    }
}