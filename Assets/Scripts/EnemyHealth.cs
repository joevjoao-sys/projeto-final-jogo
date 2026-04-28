using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Referência da UI")]
    // Arraste o componente EnemyHealthBar (que está no Canvas) para cá no Inspector
    public EnemyHealthBar healthBar; 

    void Start()
    {
        currentHealth = maxHealth;

        // Inicializa a barra de vida com 100%
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        // Atualiza a barra de vida visualmente
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        Debug.Log(gameObject.name + " tomou " + amount + " de dano! Vida restante: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " foi de arrasta pra cima!");
        Destroy(gameObject);
    }
}