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
        // Detectar tecla E para coletar o item que está olhando
        if (Input.GetKeyDown(KeyCode.E)) TentarColetar();

        // Teclas 1, 2 e 3 para usar o que estiver na Hotbar
        if (Input.GetKeyDown(KeyCode.Alpha1)) UsarItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UsarItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UsarItem(2);
    }

    // ESSA É A FUNÇÃO QUE ESTAVA FALTANDO!
    void TentarColetar()
    {
        RaycastHit hit;
        // Lança o raio para frente a partir da câmera
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distanciaColeta))
        {
            CollectibleItem item = hit.collider.GetComponent<CollectibleItem>();
           
            // Verifica se o objeto olhado realmente é um item coletável
            if (item != null && hit.collider.CompareTag("Coletavel"))
            {
                // SE FOR MUNIÇÃO: Envia direto para a arma e destrói o objeto
                if (item.tipoDoItem == CollectibleItem.ItemType.Municao)
                {
                    ArmaCapitao arma = GetComponentInChildren<ArmaCapitao>();
                    if (arma != null)
                    {
                        arma.ColetarMunicao((int)item.valorDoItem); // Abastece a reserva da arma
                        Destroy(item.gameObject); // Deleta a caixinha do chão
                        Debug.Log("Munição da reserva abastecida!");
                    }
                }
                // SE FOR RUM: Guarda em um dos slots 1, 2 ou 3 da Hotbar
                else if (item.tipoDoItem == CollectibleItem.ItemType.Rum)
                {
                    GuardarNoSlot(item);
                }
            }
        }
    }

    void GuardarNoSlot(CollectibleItem item)
    {
        for (int i = 0; i < slotsDados.Length; i++)
        {
            if (slotsDados[i] == null) // Procura slot vazio
            {
                slotsDados[i] = item;
                slotsUI[i].sprite = item.iconeDoItem; // Mostra o PNG
                slotsUI[i].color = Color.white; // Ativa a visibilidade
               
                item.Coletar(); // Faz o rum sumir do chão
                return;
            }
        }
        Debug.Log("Sua Hotbar já está cheia!");
    }

    void UsarItem(int index)
    {
        if (slotsDados[index] != null)
        {
            // Se for Rum, cura o Capitão
            if (slotsDados[index].tipoDoItem == CollectibleItem.ItemType.Rum)
            {
                playerHealth.Heal(slotsDados[index].valorDoItem);
            }

            // Limpa o slot usado
            slotsDados[index] = null;
            slotsUI[index].sprite = null;
            slotsUI[index].color = new Color(1, 1, 1, 0); // Volta a ficar transparente
        }
    }
}