using UnityEngine;

public class MorteAgua : MonoBehaviour
{
    private EnemyHealth scriptVida; // Caso seu player use o mesmo sistema de vida, ou adapte para o seu script de vida do Player

    void Start()
    {
        // Se o seu player tiver um script de vida, pegamos a referência aqui
        // scriptVida = GetComponent<EnemyHealth>();
    }

    // Esta função é chamada automaticamente pela Unity quando o Player encosta em um Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto em que o Player pisou tem a Tag "Agua"
        if (other.CompareTag("Agua"))
        {
            Afogar();
        }
    }

    /* SE O SEU JOGO FOR 2D, APAGUE A FUNÇÃO ACIMA E USE ESTA:
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Agua"))
        {
            Afogar();
        }
    }
    */

    void Afogar()
    {
        Debug.Log("O pirata foi para o armário de Davy Jones! (Morreu afogado)");
       
        // Opção 1: Se você quiser zerar a vida dele usando o script de vida
        if (scriptVida != null)
        {
            scriptVida.TakeDamage(999999f); // Dano fatal
        }
        else
        {
            // Opção 2: Reinicia a posição do jogador (Spawn) ou destrói o objeto
            // Para testar rápido, vamos apenas destruir o jogador:
            Destroy(gameObject);
           
            // DICA: Aqui você pode colocar para tocar um som de "Splash" ou abrir a tela de Game Over!
        }
    }
}