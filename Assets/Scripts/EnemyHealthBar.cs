using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthBarFill; // Arraste a imagem "Fill" aqui
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Atualiza a barra para o valor atual
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    void LateUpdate()
    {
        // Faz a barra de vida sempre olhar para a câmera do jogador (Billboard)
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);
    }
}