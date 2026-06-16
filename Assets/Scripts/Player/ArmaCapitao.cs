using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Necessário para usar Coroutines (IEnumerator)

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

    [Header("Tempo de Recarga")]
    public float tempoDeRecarga = 2.0f; // Tempo em segundos que demora para recarregar
    private bool estaRecarregando = false; // Bloqueia ações durante a recarga

    [Header("Interface (UI)")]
    public Image crosshair;
    public Color corPadrao = Color.white;
    public Color corInimigo = Color.red;
    public TextMeshProUGUI textoMunicao; 

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
        // Se estiver recarregando, o jogador não pode atirar nem tentar recarregar de novo
        if (estaRecarregando) return;

        VerificarMira();

        // Atira com o botão esquerdo
        if (Input.GetButtonDown("Fire1") && Time.time >= proximoTempoDeTiro)
        {
            proximoTempoDeTiro = Time.time + tempoEntreTiros;
            Atirar();
        }

        // Recarrega manualmente com a tecla R
        if (Input.GetKeyDown(KeyCode.R) && municaoNoPente < capacidadeDoPente)
        {
            StartCoroutine(RecarregarPente());
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
            if (balasNaReserva > 0)
            {
                StartCoroutine(RecarregarPente());
            }
            else
            {
                Debug.Log("Sem munição nenhuma!");
            }
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

        // RECARGA AUTOMÁTICA: Começa a recarga se o pente esvaziou
        if (municaoNoPente <= 0 && balasNaReserva > 0)
        {
            StartCoroutine(RecarregarPente());
        }
    }

    IEnumerator RecarregarPente()
    {
        if (balasNaReserva <= 0) yield break;

        estaRecarregando = true;
        Debug.Log("Recarregando...");

        // ESPERA: O jogo espera os segundos definidos sem mexer no texto da UI
        yield return new WaitForSeconds(tempoDeRecarga);

        // Lógica de abastecer o pente
        int balasNecessarias = capacidadeDoPente - municaoNoPente;
        int balasParaColocar = Mathf.Min(balasNecessarias, balasNaReserva);

        municaoNoPente += balasParaColocar;
        balasNaReserva -= balasParaColocar;

        estaRecarregando = false; // Libera a arma para atirar novamente
        Debug.Log("Arma recarregada!");
        AtualizarUI();
    }

    public void ColetarMunicao(int quantidade)
    {
        balasNaReserva += quantidade;
        AtualizarUI();
    }

    void AtualizarUI()
    {
        if (textoMunicao != null)
        {
            textoMunicao.text = municaoNoPente + " / " + balasNaReserva;
        }
    }
}