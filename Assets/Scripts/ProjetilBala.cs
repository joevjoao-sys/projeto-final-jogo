using UnityEngine;

public class ProjetilBala : MonoBehaviour
{
    private float danoDaBala = 25f; 
    private bool jaColidiu = false; // Evita que a bala dê dano duplo no mesmo frame

    public void ConfigurarBala(float danoConfigurado)
    {
        danoDaBala = danoConfigurado;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || jaColidiu) return;

        jaColidiu = true;

        // O SEGREDO: Desliga o colisor e a física da bala IMEDIATAMENTE no impacto
        // Isso impede que ela empurre o inimigo ou quique para trás
        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Congela a física da bala na hora
            rb.velocity = Vector3.zero;
        }

        // Procura o script de vida
        EnemyHealth vidaDoInimigo = collision.gameObject.GetComponent<EnemyHealth>();
        if (vidaDoInimigo == null)
        {
            vidaDoInimigo = collision.gameObject.GetComponentInParent<EnemyHealth>();
        }

        // Aplica o dano
        if (vidaDoInimigo != null)
        {
            vidaDoInimigo.TakeDamage(danoDaBala);
        }

        // Some com a bala do jogo
        Destroy(gameObject);
    }
}