using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ArmaCapitao : MonoBehaviour
{
    [Header("Configurações de Combate")]
    public float dano = 25f;
    public float alcance = 50f;
    public float tempoEntreTiros = 0.1f;
    private float danoOriginal; 

    [Header("Munição")]
    public int capacidadeDoPente = 8;
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
    public Image iconeBuffNaTela; 

    [Header("Configuração de Física")]
    public LayerMask layerInimigo;

    // --- NOVO: Variáveis da Bala Física ---
    [Header("Configurações do Projétil")]
    public GameObject prefabBala;   // Arraste o prefab da sua bala aqui
    public Transform firePoint;     // Arraste o objeto da ponta da arma aqui
    public float velocidadeBala = 50f;

    private float proximoTempoDeTiro;
    private Camera cameraPrincipal;
    private bool buffAtivo = false; 

    void Start()
    {
        cameraPrincipal = Camera.main;
        municaoNoPente = capacidadeDoPente;
        danoOriginal = dano; 
        
        if (iconeInimigoNaMira != null) iconeInimigoNaMira.enabled = false;
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
            if (crosshair != null) crosshair.color = buffAtivo ? Color.yellow : corPadrao;
            if (iconeInimigoNaMira != null) iconeInimigoNaMira.enabled = false;
        }
    }

    void Atirar()
    {
        if (municaoNoPente <= 0) return;

        municaoNoPente--;
        AtualizarUI();

        // --- LÓGICA NOVA: Criando o Projétil ---
        if (prefabBala != null && firePoint != null)
        {
            // Cria a bala na ponta da arma
            GameObject balaCriada = Instantiate(prefabBala, firePoint.position, firePoint.rotation);
            
            // Pega a física da bala e empurra ela para frente
            Rigidbody rb = balaCriada.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * velocidadeBala;
            }
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
        dano = danoOriginal * multiplicador;
        buffAtivo = true;
        if (iconeBuffNaTela != null) iconeBuffNaTela.enabled = true; 
        
        yield return new WaitForSeconds(duracao);

        dano = danoOriginal;
        buffAtivo = false;
        if (iconeBuffNaTela != null) iconeBuffNaTela.enabled = false; 
    }

    void AtualizarUI()
    {
        if (textoMunicao != null) textoMunicao.text = municaoNoPente + " / " + balasNaReserva;
    }
}