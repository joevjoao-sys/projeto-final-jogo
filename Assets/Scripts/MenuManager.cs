using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject gameHUDPanel;  

    [Header("Game Elements to Control")]
    public GameObject playerObject;      // Arraste o seu Player aqui
    public GameObject enemySpawnerObject; // Arraste o gerador de inimigos/ondas aqui

    void Start()
    {
        // Garante as telas certas no início
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gameHUDPanel != null) gameHUDPanel.SetActive(false);

        // Em vez de congelar o tempo, desativamos o jogador e o gerador de monstros
        if (playerObject != null) playerObject.SetActive(false);
        if (enemySpawnerObject != null) enemySpawnerObject.SetActive(false);

        // Deixa o tempo normal (1), assim nenhum cronômetro de spawn quebra
        Time.timeScale = 1f;

        // Libera o mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        // Inverte as telas
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameHUDPanel != null) gameHUDPanel.SetActive(true);

        // ATIVA o jogador e o gerador de monstros agora que o jogo começou!
        if (playerObject != null) playerObject.SetActive(true);
        if (enemySpawnerObject != null) enemySpawnerObject.SetActive(true);

        // Prende o mouse para jogar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}