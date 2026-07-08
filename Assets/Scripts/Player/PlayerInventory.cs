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
        if (Input.GetKeyDown(KeyCode.E)) TentarColetar();

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
            
            if (item != null && hit.collider.CompareTag("Coletavel"))
            {
                if (item.tipoDoItem == CollectibleItem.ItemType.Municao)
                {
                    ArmaCapitao arma = GetComponentInChildren<ArmaCapitao>();
                    if (arma != null)
                    {
                        arma.ColetarMunicao((int)item.valorDoItem); 
                        Destroy(item.gameObject); 
                        Debug.Log("Munição da reserva abastecida!");
                    }
                }
                else if (item.tipoDoItem == CollectibleItem.ItemType.Rum || item.tipoDoItem == CollectibleItem.ItemType.Pimenta)
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
            if (slotsDados[i] == null) 
            {
                slotsDados[i] = item;
                slotsUI[i].sprite = item.iconeDoItem; 
                slotsUI[i].color = Color.white; 
                
                item.Coletar(); 
                return;
            }
        }
        Debug.Log("Sua Hotbar já está cheia!");
    }

    void UsarItem(int index)
    {
        if (slotsDados[index] != null)
        {
            if (slotsDados[index].tipoDoItem == CollectibleItem.ItemType.Rum)
            {
                playerHealth.Heal(slotsDados[index].valorDoItem);
            }
            else if (slotsDados[index].tipoDoItem == CollectibleItem.ItemType.Pimenta)
            {
                ArmaCapitao arma = GetComponentInChildren<ArmaCapitao>();
                if (arma != null)
                {
                    arma.AtivarBuffDeDano(slotsDados[index].multiplicadorDeDano, slotsDados[index].tempoDeEfeito);
                }
            }

            // Limpa o slot usado
            slotsDados[index] = null;
            slotsUI[index].sprite = null;
            slotsUI[index].color = new Color(1, 1, 1, 0); 
        }
    }
}