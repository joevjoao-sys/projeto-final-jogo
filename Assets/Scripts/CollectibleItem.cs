using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum ItemType { Rum, Municao, Chave }
    public ItemType tipoDoItem;

    void OnTriggerEnter(Collider other)
    {
        // Verifica se quem passou por dentro foi o Jogador (usando a Tag que definimos antes)
        if (other.CompareTag("Player"))
        {
            Coletar(other.gameObject);
        }
    }

    void Coletar(GameObject player)
    {
        switch (tipoDoItem)
        {
            case ItemType.Rum:
                Debug.Log("Coletou Rum! Curando o Capitão (Lógica futura)...");
                // Aqui chamariamos player.GetComponent<PlayerHealth>().Heal(20);
                break;
            case ItemType.Municao:
                Debug.Log("Coletou Munição! Carregando Bacamarte...");
                break;
        }

        // Som de coleta entraria aqui

        // Destrói o item da cena
        Destroy(gameObject);
    }
}