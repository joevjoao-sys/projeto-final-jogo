using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum ItemType { Rum, Municao, Chave, Pimenta }
    
    [Header("Configurações do Item")]
    public ItemType tipoDoItem;
    public string nomeDoItem;
    public Sprite iconeDoItem; // O PNG que aparecerá na sua Hotbar
    
    [Tooltip("Usado para cura (Rum) ou quantidade de balas (Munição)")]
    public float valorDoItem = 25f; 

    [Header("Configurações do Buff (Apenas Pimenta)")]
    public float multiplicadorDeDano = 2f; // Dobra o dano
    public float tempoDeEfeito = 5f; // Dura 5 segundos

    public void Coletar()
    {
        Debug.Log(nomeDoItem + " coletado!");
        gameObject.SetActive(false);
    }
}