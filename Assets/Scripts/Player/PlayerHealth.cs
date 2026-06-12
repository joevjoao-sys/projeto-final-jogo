using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necessário para recarregar a cena

public class PlayerHealth : MonoBehaviour
{
    [Header("Saúde do Capitão")]
    public float maxHealth = 100f;
    public float currentHealth; // Alterado de private para public para o inspector, ou mantenha private se preferir

    [Header("Configurações do Efeito de Dano")]
    [Tooltip("Arraste a Imagem preta da interface (UI) para cá")]
    public Image damageOverlay;
   
    [Tooltip("Escuridão máxima da tela quando a vida estiver quase zero (de 0 a 1)")]
    [Range(0f, 1f)]
    public float maxOverlayAlpha = 0.85f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateDamageOverlay(); // Garante que a tela comece normal
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
       
        // Mathf.Clamp impede que a vida fique negativa
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("O Capitão foi atingido! Vida restante: " + currentHealth);

        UpdateDamageOverlay(); // Atualiza a tela preta

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
       
        // Mathf.Clamp impede que a vida passe de 100
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Tomou um Rum! Vida curada. Vida atual: " + currentHealth);

        UpdateDamageOverlay(); // Clareia a tela preta
    }

    private void UpdateDamageOverlay()
    {
        if (damageOverlay != null)
        {
            float healthRatio = currentHealth / maxHealth;
            float alphaAmount = (1f - healthRatio) * maxOverlayAlpha;

            Color overlayColor = damageOverlay.color;
            overlayColor.a = alphaAmount;
            damageOverlay.color = overlayColor;
        }
        else
        {
            Debug.LogWarning("Aviso: A imagem do 'Damage Overlay' não foi atribuída no Inspector da Unity!");
        }
    }

    void Die()
    {
        Debug.Log("O navio afundou... Game Over.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}