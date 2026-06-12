using UnityEngine;
using UnityEngine.UI; // Adicionado para controlar a interface (UI)

public class EnemyHealth : MonoBehaviour
{
    [Header("Saúde do Monstro")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Interface")]
    public Image barraDeVida; // Onde você vai arrastar a imagem "Filled"
    public Transform canvasDaVida; // Onde você vai arrastar o objeto principal do Canvas

    [Header("Configurações de Drop")]
    public GameObject rumPrefab; // Arraste o prefab do Rum aqui
    public GameObject municaoPrefab; // Arraste o prefab da Caixa de Munição aqui
    [Range(0f, 100f)]
    public float chanceDeDrop = 40f; // Chance geral de dropar algo (40%)

    void Start()
    {
        currentHealth = maxHealth; // Nasce com a vida cheia
       
        // Garante que a barra comece cheia visualmente
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = 1f;
        }
    }

    void Update()
    {
        // Faz o Canvas sempre olhar para a câmera do jogador (essencial para FPS)
        if (canvasDaVida != null)
        {
            canvasDaVida.LookAt(canvasDaVida.position + Camera.main.transform.rotation * Vector3.forward,
                                Camera.main.transform.rotation * Vector3.up);
        }
    }

    // A arma chama essa função quando o Raycast bate nele
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " tomou tiro! Vida: " + currentHealth);

        // A Mágica acontece aqui: a barra diminui proporcionalmente
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Sistema de sorteio para os drops
        float sorteio = Random.Range(0f, 100f);
       
        if (sorteio <= chanceDeDrop)
        {
            // 50% de chance de ser Rum, 50% de ser Munição
            if (Random.value > 0.5f && rumPrefab != null)
            {
                Instantiate(rumPrefab, transform.position + Vector3.up, Quaternion.identity);
                Debug.Log(gameObject.name + " dropou um Rum!");
            }
            else if (municaoPrefab != null)
            {
                Instantiate(municaoPrefab, transform.position + Vector3.up, Quaternion.identity);
                Debug.Log(gameObject.name + " dropou Munição!");
            }
        }

        Debug.Log(gameObject.name + " foi destruído!");
        // Destrói o inimigo da cena
        Destroy(gameObject);
    }
}