using UnityEngine;

public class PuddleFade : MonoBehaviour
{
    void Start()
    {
        // A poça se destrói sozinha após 2 segundos
        Destroy(gameObject, 2f);
    }
}