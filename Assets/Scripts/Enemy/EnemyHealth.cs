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

    [Header("Configurações de Drop")]
    public GameObject rumPrefab; // Arraste o prefab da garrafa de Rum aqui
    [Range(0, 100)]
    public float chanceDeDrop = 30f; // 30% de chance de cair Rum

    void Start()
    {
        currentHealth = maxHealth;
        if (barraDeVida != null) barraDeVida.fillAmount = 1f;
    }

    void Update()
    {
        if (canvasDaVida != null)
        {
            canvasDaVida.LookAt(canvasDaVida.position + Camera.main.transform.rotation * Vector3.forward,
                                Camera.main.transform.rotation * Vector3.up);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (barraDeVida != null) barraDeVida.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0f) Die();
    }

 void Die()
    {
        // Sua Lógica de Drop (Intacta!)
        float sorteio = Random.Range(0f, 100f);
        if (sorteio <= chanceDeDrop && rumPrefab != null)
        {
            // Cria o Rum na posição do inimigo
            Instantiate(rumPrefab, transform.position + Vector3.up, Quaternion.identity);
        }

        // NOVO: Avisar o Gerenciador de Hordas que este monstro morreu!
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.EnemyDefeated();
        }

        Debug.Log(gameObject.name + " foi destruído!");
        Destroy(gameObject);
    }