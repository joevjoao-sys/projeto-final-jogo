using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importante para usar o texto de alta definição

public class ArmaCapitao : MonoBehaviour
{
    [Header("Configurações de Combate")]
    public float dano = 25f;
    public float alcance = 50f;
    public float tempoEntreTiros = 0.5f;

    [Header("Munição")]
    public int capacidadeDoPente = 6;
    private int municaoNoPente;
    public int balasNaReserva = 24; // Balas totais que o jogador carrega

    [Header("Interface (UI)")]
    public Image crosshair;
    public Color corPadrao = Color.white;
    public Color corInimigo = Color.red;
    public TextMeshProUGUI textoMunicao; // Arraste o texto da UI aqui

    [Header("Configuração de Física")]
    public LayerMask layerInimigo;

    private float proximoTempoDeTiro;
    private Camera cameraPrincipal;

    void Start()
    {
        cameraPrincipal = Camera.main;
        municaoNoPente = capacidadeDoPente;
       
        if (crosshair != null) crosshair.color = corPadrao;
        AtualizarUI();
    }

    void Update()
    {
        VerificarMira();

        // Atira com o botão esquerdo
        if (Input.GetButtonDown("Fire1") && Time.time >= proximoTempoDeTiro)
        {
            proximoTempoDeTiro = Time.time + tempoEntreTiros;
            Atirar();
        }

        // Recarrega manualmente com a tecla R se o pente não estiver cheio
        if (Input.GetKeyDown(KeyCode.R) && municaoNoPente < capacidadeDoPente)
        {
            RecarregarPente();
        }
    }

    void VerificarMira()
    {
        if (crosshair == null) return;
        RaycastHit hit;
        Ray raio = cameraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(raio, out hit, alcance, layerInimigo))
            crosshair.color = corInimigo;
        else
            crosshair.color = corPadrao;
    }

    void Atirar()
    {
        if (municaoNoPente <= 0)
        {
            Debug.Log("Pente vazio! Aperte R para recarregar.");
            return;
        }

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
        if (balasNaReserva <= 0)
        {
            Debug.Log("Sem munição na reserva para recarregar!");
            return;
        }

        int balasNecessarias = capacidadeDoPente - municaoNoPente;
        int balasParaColocar = Mathf.Min(balasNecessarias, balasNaReserva);

        municaoNoPente += balasParaColocar;
        balasNaReserva -= balasParaColocar;

        Debug.Log("Arma recarregada!");
        AtualizarUI();
    }

    // Função que o item do chão vai chamar quando o player pegar munição
    public void ColetarMunicao(int quantidade)
    {
        balasNaReserva += quantidade;
        AtualizarUI();
    }

    // Mantém a interface bonita e atualizada: Ex: 6 / 24
    void AtualizarUI()
    {
        if (textoMunicao != null)
        {
            textoMunicao.text = municaoNoPente + " / " + balasNaReserva;
        }
    }
}