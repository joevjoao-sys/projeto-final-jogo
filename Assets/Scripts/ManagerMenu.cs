using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerMenu : MonoBehaviour
{
    [Header("Telas do Menu")]
    public GameObject painelPrincipal; // Onde ficam os botões de Iniciar, Controles e Sair
    public GameObject painelControles; // A sua nova tabela

    void Start()
    {
        // Garante que o jogo comece na tela certa
        if (painelPrincipal != null) painelPrincipal.SetActive(true);
        if (painelControles != null) painelControles.SetActive(false);
    }

    public void IniciarJogo()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

    // --- NOVAS FUNÇÕES PARA OS CONTROLES ---
    public void AbrirControles()
    {
        painelPrincipal.SetActive(false);
        painelControles.SetActive(true);
    }

    public void FecharControles()
    {
        painelControles.SetActive(false);
        painelPrincipal.SetActive(true);
    }
    // ---------------------------------------

    public void SairDoJogo()
    {
        Debug.Log("Fechando Maré de Sangue...");
        Application.Quit();
    }
}