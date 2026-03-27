using UnityEngine;

public class TrailFade : MonoBehaviour
{
    public float lifetime = 0.05f; // Quão rápido o rastro some

    void Start()
    {
        // Destrói o objeto após o tempo de vida
        Destroy(gameObject, lifetime);
    }
}