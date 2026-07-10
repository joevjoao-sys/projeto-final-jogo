using UnityEngine;

public class ProjetilBala : MonoBehaviour
{
    private float danoDaBala = 25f; 

    // 1. Essa função recebe o dano vindo da arma
    public void ConfigurarBala(float danoConfigurado)
    {
        danoDaBala = danoConfigurado;
    } // <-- ESSA CHAVE PRECISA FECHAR AQUI!

    // 2. Essa função roda quando a bala bate em algo
    private void OnTriggerEnter(Collider other)
    {
        // Ignora o jogador
        if (other.CompareTag("Player")) return;

        // AVISO 1: Mostra no Console em qual objeto a bala bateu
        Debug.Log("A bala colidiu com: " + other.gameObject.name);

        // Tenta pegar o componente de vida do inimigo
        EnemyHealth vidaDoInimigo = other.GetComponent<EnemyHealth>();

        if (vidaDoInimigo != null)
        {
            vidaDoInimigo.TakeDamage(danoDaBala);
            // AVISO 2: Confirma se o dano foi enviado
            Debug.Log($"Dano de {danoDaBala} enviado com sucesso para {other.gameObject.name}!");
        }
        else
        {
            // AVISO 3: Alerta se a bala bateu no inimigo mas não achou o script de vida
            Debug.LogWarning("Bateu, mas NÃO encontrou o script 'EnemyHealth' nesse objeto.");
        }

        // Destrói a bala após o impacto
        Destroy(gameObject);
    }
}