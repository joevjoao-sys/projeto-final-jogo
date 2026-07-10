using UnityEngine;

public class MorteAgua : MonoBehaviour
{
    // --- ATUALIZADO: Agora aponta para o script de vida correto do seu Player ---
    private PlayerHealth scriptVidaPlayer; 

    void Start()
    {
        // Busca o script de vida do jogador que está grudado neste mesmo objeto
        scriptVidaPlayer = GetComponent<PlayerHealth>();
        
        // Caso o PlayerHealth esteja em outro lugar, um backup para encontrar ele:
        if (scriptVidaPlayer == null)
        {
            scriptVidaPlayer = FindObjectOfType<PlayerHealth>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto em que o Player pisou tem a Tag "Agua"
        if (other.CompareTag("Agua"))
        {
            Afogar();
        }
    }

    void Afogar()
    {
        Debug.Log("O pirata foi para o armário de Davy Jones! (Morreu afogado)");
        
        // Se encontramos o script de vida do Capitão, damos um dano fatal nele
        if (scriptVidaPlayer != null)
        {
            // Substitua 'TakeDamage' pelo nome exato da função de dano do seu PlayerHealth se for diferente
            scriptVidaPlayer.TakeDamage(999999f); 
        }
        else
        {
            // Se o script de vida não for encontrado (por segurança), 
            // apenas desativa o movimento do player em vez de deletar o objeto com a câmera
            Debug.LogError("MorteAgua: O script PlayerHealth não foi encontrado no personagem!");
            
            // Como plano B para testes, teletransporta o player para cima para ele não sumir da tela
            transform.position = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
        }
    }
}