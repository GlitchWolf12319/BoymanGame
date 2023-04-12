using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [SerializeField] private float duration = 2.0f;

    void Start()
    {
        // Invoke the DestroyObject method after 'duration' seconds
        Invoke("DestroyObject", duration);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null){         
                Movement[] move = FindObjectsOfType<Movement>();
                foreach(Movement Move in move){
                Move.StopEverything();
                Move.enabled = false;
            }
        }
    }

    void DestroyObject()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null){         
                Movement[] move = FindObjectsOfType<Movement>();
                foreach(Movement Move in move){
                Move.enabled = true;
            }
        }
        // Destroy the object this script is attached to
        Destroy(gameObject);
    }
}