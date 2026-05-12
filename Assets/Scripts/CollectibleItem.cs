using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum ItemType { Rum, Municao, Chave }
   
    [Header("Configurações do Item")]
    public ItemType tipoDoItem;
    public string nomeDoItem;
    public Sprite iconeDoItem; // O PNG que aparecerá na sua Hotbar
    public float valorDoItem = 25f; // Quanto cura ou quanta munição dá

    // Chamado pelo PlayerInventory quando você aperta "E"
    public void Coletar()
    {
        Debug.Log(nomeDoItem + " coletado!");
        // Desativa o item para ele "sumir" do mundo e ir para o inventário
        gameObject.SetActive(false);
    }
}