using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para recarregar a cena no Game Over

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

    void Die()
    {
        Debug.Log("O navio afundou... Game Over.");
       
        // Recarrega a cena atual para podermos testar o jogo continuamente
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}