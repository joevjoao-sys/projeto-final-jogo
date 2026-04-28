using UnityEngine;

public class TrailFade : MonoBehaviour
{
    public float lifetime = 0.2f; // Aumentamos o tempo de vida
    private LineRenderer lr;
    private float alpha = 1f;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        // Destrói o objeto no final do tempo de vida
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (lr != null)
        {
            // Reduz o alpha ao longo do tempo para criar o efeito de "sumiço" suave (Fade Out)
            alpha -= Time.deltaTime / lifetime;
           
            // Aplica a nova transparência na cor da linha
            Color color = lr.startColor;
            color.a = alpha;
            lr.startColor = color;
            lr.endColor = color;
        }
    }
}
