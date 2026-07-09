using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Telas do Menu Inicial / Pause")]
    public GameObject painelMenuPrincipal; 
    public GameObject painelControles;     

    [Header("Telas de Fim de Jogo")]
    public GameObject painelMorte;
    public GameObject painelCreditos; 

    private bool jogadorMorto = false;
    private bool jogoFinalizado = false;

    void Start()
    {
        // O jogo abre congelado no Menu Principal
        Time.timeScale = 0f; 
        
        if (painelMenuPrincipal != null) painelMenuPrincipal.SetActive(true);
        
        if (painelControles != null) painelControles.SetActive(false);
        if (painelMorte != null) painelMorte.SetActive(false);
        if (painelCreditos != null) painelCreditos.SetActive(false);

        LiberarMouse();
    }

    void Update()
    {
        // O botão ESC agora abre ou fecha o Menu Principal direto
        if (Input.GetKeyDown(KeyCode.Escape) && !jogadorMorto && !jogoFinalizado)
        {
            if (painelMenuPrincipal.activeSelf)
            {
                IniciarOuContinuarJogo();
            }
            else
            {
                IrParaMenuPrincipal();
            }
        }
    }

    public void IniciarOuContinuarJogo()
    {
        if (jogadorMorto || jogoFinalizado)
        {
            ReiniciarFase();
            return;
        }
        
        painelMenuPrincipal.SetActive(false);
        painelControles.SetActive(false);
        
        Time.timeScale = 1f; 
        TravarMouse();
    }

    public void IrParaMenuPrincipal()
    {
        painelMenuPrincipal.SetActive(true);
        painelControles.SetActive(false);
        
        Time.timeScale = 0f;
        LiberarMouse();
    }

    public void AbrirControles()
    {
        painelMenuPrincipal.SetActive(false);
        painelControles.SetActive(true);
    }

    public void FecharControles()
    {
        painelControles.SetActive(false);
        painelMenuPrincipal.SetActive(true);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    public void MostrarMenuMorte()
    {
        jogadorMorto = true;
        painelMorte.SetActive(true);
        Time.timeScale = 0f;
        LiberarMouse();
    }

    public void MostrarCreditos()
    {
        jogoFinalizado = true;
        if (painelCreditos != null) painelCreditos.SetActive(true);
        Time.timeScale = 0f;
        LiberarMouse();
    }

    public void ReiniciarFase()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LiberarMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void TravarMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}