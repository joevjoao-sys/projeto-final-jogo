using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Telas da UI")]
    public GameObject painelPause;
    public GameObject painelMorte;

    private bool jogoPausado = false;
    private bool jogadorMorto = false;

    void Start()
    {
        Time.timeScale = 1f;
        if (painelPause != null) painelPause.SetActive(false);
        if (painelMorte != null) painelMorte.SetActive(false);

        TravarMouse();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !jogadorMorto)
        {
            if (jogoPausado) RetomarJogo();
            else PausarJogo();
        }
    }

    public void PausarJogo()
    {
        jogoPausado = true;
        painelPause.SetActive(true);
        Time.timeScale = 0f;
        LiberarMouse();
    }

    public void RetomarJogo()
    {
        jogoPausado = false;
        painelPause.SetActive(false);
        Time.timeScale = 1f;
        TravarMouse();
    }

    public void MostrarMenuMorte()
    {
        jogadorMorto = true;
        painelMorte.SetActive(true);
        Time.timeScale = 0f;
        LiberarMouse();
    }

    public void ReiniciarFase()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VoltarAoMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
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