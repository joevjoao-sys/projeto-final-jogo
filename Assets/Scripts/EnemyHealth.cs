using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        // O inimigo nasce com a vida cheia
        currentHealth = maxHealth;
    }

    // Essa é a função que a nossa arma vai chamar quando o Raycast acertar
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " tomou " + amount + " de dano! Vida restante: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Aqui no futuro podemos colocar um efeito de explosão de sangue (partículas) ou som
        Debug.Log(gameObject.name + " foi de arrasta pra cima!");
       
        // Destrói o objeto da cena
        Destroy(gameObject);
    }
}