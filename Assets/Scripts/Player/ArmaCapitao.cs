using UnityEngine;
using UnityEngine.UI; // Obrigatório para gerenciar a UI
using TMPro;

public class ArmaCapitao : MonoBehaviour
{
    [Header("Configurações de Combate")]
    public float dano = 25f;
    public float alcance = 50f;
    public float tempoEntreTiros = 0.5f;

    [Header("Munição")]
    public int capacidadeDoPente = 6;
    private int municaoNoPente;
    public int balasNaReserva = 24;

    [Header("Interface (UI)")]
    public Image crosshair; // Sua mira padrão
    public Color corPadrao = Color.white;
    public Color corInimigo = Color.red;
   
    // --- NOVA VARIÁVEL AQUI ---
    [Tooltip("Imagem extra que aparece quando mira no inimigo (ex: uma caveira, um x)")]
    public Image iconeInimigoNaMira;
   
    public TextMeshProUGUI textoMunicao;

    [Header("Configuração de Física")]
    public LayerMask layerInimigo;

    private float proximoTempoDeTiro;
    private Camera cameraPrincipal;

    void Start()
    {
        cameraPrincipal = Camera.main;
        municaoNoPente = capacidadeDoPente;
       
        // Garante que o ícone extra comece escondido
        if (iconeInimigoNaMira != null)
        {
            iconeInimigoNaMira.enabled = false;
        }

        if (crosshair != null) crosshair.color = corPadrao;
        AtualizarUI();
    }

    void Update()
    {
        VerificarMira();

        if (Input.GetButtonDown("Fire1") && Time.time >= proximoTempoDeTiro)
        {
            proximoTempoDeTiro = Time.time + tempoEntreTiros;
            Atirar();
        }

        if (Input.GetKeyDown(KeyCode.R) && municaoNoPente < capacidadeDoPente)
        {
            RecarregarPente();
        }
    }

    // --- LÓGICA ATUALIZADA AQUI ---
    void VerificarMira()
    {
        RaycastHit hit;
        Ray raio = cameraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // Se o raio atingiu o layer Enemy
        if (Physics.Raycast(raio, out hit, alcance, layerInimigo))
        {
            // 1. Muda a cor da mira principal
            if (crosshair != null) crosshair.color = corInimigo;

            // 2. MOSTRA o ícone extra
            if (iconeInimigoNaMira != null) iconeInimigoNaMira.enabled = true;
        }
        else
        {
            // 1. Volta a cor padrão da mira
            if (crosshair != null) crosshair.color = corPadrao;

            // 2. ESCONDE o ícone extra
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

    void RecarregarPente()
    {
        if (balasNaReserva <= 0) return;
        int balasNecessarias = capacidadeDoPente - municaoNoPente;
        int balasParaColocar = Mathf.Min(balasNecessarias, balasNaReserva);
        municaoNoPente += balasParaColocar;
        balasNaReserva -= balasParaColocar;
        AtualizarUI();
    }

    public void ColetarMunicao(int quantidade)
    {
        balasNaReserva += quantidade;
        AtualizarUI();
    }

    void AtualizarUI()
    {
        if (textoMunicao != null) textoMunicao.text = municaoNoPente + " / " + balasNaReserva;
    }
}