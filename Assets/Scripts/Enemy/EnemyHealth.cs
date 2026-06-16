using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Saúde do Monstro")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Interface")]
    public Image barraDeVida;
    public Transform canvasDaVida;
    private Camera mainCamera; // Otimização: guarda a referência da câmera

    [Header("Configurações de Drop")]
    public GameObject rumPrefab;
    public GameObject municaoPrefab;
    [Range(0f, 100f)]
    public float chanceDeDrop = 40f;

    void Start()
    {
        currentHealth = maxHealth;
        mainCamera = Camera.main; // Acha a câmera uma vez só no início

        AtualizarBarraDeVida();
    }

    void LateUpdate()
    {
        // LateUpdate é mais suave para UIs que seguem a câmera
        if (canvasDaVida != null && mainCamera != null)
        {
            canvasDaVida.LookAt(canvasDaVida.position + mainCamera.transform.forward);
        }
    }

    public void TakeDamage(float amount)
    {
        // Se o monstro já foi de arrasta pra cima, ignora o dano extra
        if (currentHealth <= 0) return;

        // Tira a vida, mas impede que o valor fique menor que 0 ou maior que o máximo
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        Debug.Log($"{gameObject.name} tomou um golpe! Vida: {currentHealth}");

        AtualizarBarraDeVida();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void AtualizarBarraDeVida()
    {
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        GerarDrop();
       
        Debug.Log($"{gameObject.name} foi pro fundo do mar!");
        Destroy(gameObject); // Destrói o inimigo
    }

    private void GerarDrop()
    {
        float sorteio = Random.Range(0f, 100f);
       
        if (sorteio <= chanceDeDrop)
        {
            // Sorteia 50/50 e escolhe o prefab correspondente em uma linha só
            GameObject itemSorteado = (Random.value > 0.5f) ? rumPrefab : municaoPrefab;

            // Se o prefab não estiver vazio lá no Inspector, ele cria o item
            if (itemSorteado != null)
            {
                Instantiate(itemSorteado, transform.position + Vector3.up, Quaternion.identity);
                Debug.Log($"Saque à vista! Dropou {itemSorteado.name}");
            }
        }
    }
}