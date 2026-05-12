using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Saúde do Capitão")]
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("O Capitão foi atingido! Vida restante: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // NOVA FUNÇÃO: Adicionada para o Rum curar o capitão
    public void Heal(float amount)
    {
        currentHealth += amount;
       
        // Impede que a vida ultrapasse o limite máximo (100f)
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        Debug.Log("Tomou um Rum! Vida curada. Vida atual: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("O navio afundou... Game Over.");
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}