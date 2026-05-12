using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public float distanciaColeta = 3f;
   
    [Header("Sua Hotbar (Arraste os 3 Slots aqui)")]
    public Image[] slotsUI;
   
    private CollectibleItem[] slotsDados = new CollectibleItem[3];
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // 1. Detectar tecla E para coletar
        if (Input.GetKeyDown(KeyCode.E)) TentarColetar();

        // 2. Teclas 1, 2 e 3 para usar o que estiver no slot
        if (Input.GetKeyDown(KeyCode.Alpha1)) UsarItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UsarItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UsarItem(2);
    }

    void TentarColetar()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distanciaColeta))
        {
            CollectibleItem item = hit.collider.GetComponent<CollectibleItem>();
           
            // Verifica se é um item e se tem a Tag "Coletavel"
            if (item != null && hit.collider.CompareTag("Coletavel"))
            {
                GuardarNoSlot(item);
            }
        }
    }

    void GuardarNoSlot(CollectibleItem item)
    {
        for (int i = 0; i < slotsDados.Length; i++)
        {
            if (slotsDados[i] == null) // Encontrou slot vazio
            {
                slotsDados[i] = item;
                slotsUI[i].sprite = item.iconeDoItem; // Coloca o PNG na UI
                slotsUI[i].color = Color.white; // Deixa o PNG visível
               
                item.Coletar(); // Executa a função de sumir do chão
                return;
            }
        }
        Debug.Log("Inventário cheio!");
    }

    void UsarItem(int index)
    {
        if (slotsDados[index] != null)
        {
            // Se for Rum, cura o player
            if (slotsDados[index].tipoDoItem == CollectibleItem.ItemType.Rum)
            {
                playerHealth.Heal(slotsDados[index].valorDoItem);
            }
            // Se for munição, você pode adicionar a lógica de recarga aqui futuramente

            // Limpa o slot após o uso
            slotsDados[index] = null;
            slotsUI[index].sprite = null;
            slotsUI[index].color = new Color(1, 1, 1, 0); // Volta a ficar transparente
        }
    }
}