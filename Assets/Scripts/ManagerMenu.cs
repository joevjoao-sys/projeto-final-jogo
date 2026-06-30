using UnityEngine;
using UnityEngine.SceneManagement; // Obrigatório para trocar de cenas

public class ManagerMenu : MonoBehaviour
{
    // Função chamada pelo botão "Iniciar"
    public void IniciarJogo()
    {
        // Certifique-se de colocar exatamente o nome da sua cena onde o jogo acontece
        SceneManager.LoadScene("SampleScene");
    }

    // Função chamada pelo botão "Sair"
    public void SairDoJogo()
    {
        Debug.Log("Fechando Maré de Sangue...");
        Application.Quit(); // Só funciona na build final exportada
    }
}
