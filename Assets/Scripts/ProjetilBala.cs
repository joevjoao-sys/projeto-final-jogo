using UnityEngine;

public class ProjetilBala : MonoBehaviour
{
    private float danoDaBala = 25f; // Valor padrão caso não receba da arma

    // Função para a arma passar o dano correto para a bala ao nascer
    public void ConfigurarBala(float danoConfigurado)
    {
        danoDaBala = danoConfigurado;
    }

    // Essa função roda automaticamente quando a bala encosta em algo (com Is Trigger marcado)
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que a bala tocou tem o componente de vida do inimigo
        EnemyHealth vidaDoInimigo = other.GetComponent<EnemyHealth>();

        if (vidaDoInimigo != null)
        {
            // Dá o dano no inimigo!
            vidaDoInimigo.TakeDamage(danoDaBala);
        }

        // Destrói a bala na mesma hora para ela não atravessar ou ficar caída
        Destroy(gameObject);
    }
}