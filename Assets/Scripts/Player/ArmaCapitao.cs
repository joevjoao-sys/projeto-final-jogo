using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ArmaCapitao : MonoBehaviour
{
    [Header("Configurações de Combate")]
    public float dano = 25f;
    public float alcance = 50f;
    public float tempoEntreTiros = 0.5f;
    private float danoOriginal; 

    [Header("Munição")]
    public int capacidadeDoPente = 6;
    private int municaoNoPente;
    public int balasNaReserva = 24;
    public float tempoDeRecarga = 1.5f; 
    private bool isReloading = false; 

    [Header("Interface (UI)")]
    public Image crosshair;
    public Color corPadrao = Color.white;
    public Color corInimigo = Color.red;
    public Image iconeInimigoNaMira;
    public TextMeshProUGUI textoMunicao;
    
    // --- NOVO: Ícone que avisa que o buff está rolando ---
    public Image iconeBuffNaTela; 

    [Header("Configuração de Física")]
    public LayerMask layerInimigo;

    private float proximoTempoDeTiro;
    private Camera cameraPrincipal;
    
    // --- NOVO: Variável para saber se o Capitão tá furioso ---
    private bool buffAtivo = false; 

    void Start()
    {
        cameraPrincipal = Camera.main;
        municaoNoPente = capacidadeDoPente;
        danoOriginal = dano; 
        
        if (iconeInimigoNaMira != null) iconeInimigoNaMira.enabled = false;
        
        // Garante que o ícone do buff comece invisível
        if (iconeBuffNaTela != null) iconeBuffNaTela.enabled = false; 
        
        if (crosshair != null) crosshair.color = corPadrao;
        
        AtualizarUI();
    }

    void Update()
    {
        if (isReloading) return;

        VerificarMira();

        if (municaoNoPente <= 0 && balasNaReserva > 0)
        {
            StartCoroutine(RotinaDeRecarga());
            return; 
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= proximoTempoDeTiro)
        {
            proximoTempoDeTiro = Time.time + tempoEntreTiros;
            Atirar();
        }

        if (Input.GetKeyDown(KeyCode.R) && municaoNoPente < capacidadeDoPente && balasNaReserva > 0)
        {
            StartCoroutine(RotinaDeRecarga());
        }
    }

    void VerificarMira()
    {
        RaycastHit hit;
        Ray raio = cameraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(raio, out hit, alcance, layerInimigo))
        {
            if (crosshair != null) crosshair.color = corInimigo;
            if (iconeInimigoNaMira != null) iconeInimigoNaMira.enabled = true;
        }
        else
        {
            // LÓGICA NOVA DA MIRA: 
            // Se tiver com buff, fica Amarela. Se não, volta pro Branco padrão.
            if (crosshair != null) crosshair.color = buffAtivo ? Color.yellow : corPadrao;
            if (iconeInimigoNaMira != null) iconeInimigoNaMira.enabled = false;
        }
    }

    void Atirar()
    {
        if (municaoNoPente <= 0) return;

        municaoNoPente--;
        AtualizarUI();

        RaycastHit hit;
        Ray raio = cameraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(raio, out hit, alcance, layerInimigo))
        {
            EnemyHealth vidaDoInimigo = hit.transform.GetComponent<EnemyHealth>();
            if (vidaDoInimigo != null) vidaDoInimigo.TakeDamage(dano);
        }
    }

    IEnumerator RotinaDeRecarga()
    {
        isReloading = true;
        yield return new WaitForSeconds(tempoDeRecarga);

        int balasNecessarias = capacidadeDoPente - municaoNoPente;
        int balasParaColocar = Mathf.Min(balasNecessarias, balasNaReserva);
        
        municaoNoPente += balasParaColocar;
        balasNaReserva -= balasParaColocar;
        
        AtualizarUI();
        isReloading = false; 
    }

    public void ColetarMunicao(int quantidade)
    {
        balasNaReserva += quantidade;
        AtualizarUI();
    }

    public void AtivarBuffDeDano(float multiplicador, float duracao)
    {
        StartCoroutine(RotinaBuffDeDano(multiplicador, duracao));
    }

    private IEnumerator RotinaBuffDeDano(float multiplicador, float duracao)
    {
        // LIGA O MODO FURIOSO
        dano = danoOriginal * multiplicador;
        buffAtivo = true;
        if (iconeBuffNaTela != null) iconeBuffNaTela.enabled = true; // Aparece a pimenta na tela
        
        Debug.Log($"Modo Furioso! Dano subiu para: {dano}");

        yield return new WaitForSeconds(duracao);

        // DESLIGA O MODO FURIOSO
        dano = danoOriginal;
        buffAtivo = false;
        if (iconeBuffNaTela != null) iconeBuffNaTela.enabled = false; // Some a pimenta da tela
        
        Debug.Log("O efeito passou. Dano normalizado.");
    }

    void AtualizarUI()
    {
        if (textoMunicao != null) textoMunicao.text = municaoNoPente + " / " + balasNaReserva;
    }
}