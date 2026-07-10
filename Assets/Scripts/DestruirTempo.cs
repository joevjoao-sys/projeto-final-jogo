using UnityEngine;

public class DestruirTempo : MonoBehaviour
{
    void Start()
    {
        // Destrói este objeto (a bala) após 3 segundos
        Destroy(gameObject, 3f); 
    }
}