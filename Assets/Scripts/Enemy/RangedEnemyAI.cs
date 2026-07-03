using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAI : MonoBehaviour
{
    [Header("Componentes")]
    public NavMeshAgent agent;
    private Transform player;
   
    [Header("Movimentação")]
    public float distanciaParaParar = 10f;
    public float velocidadeDeGiro = 5f; // Velocidade com que ele vira o corpo
   
    [Header("Configurações de Tiro")]
    public float distanciaDeTiro = 15f;
    public float tempoEntreTiros = 2f;
    public GameObject projetilPrefab;
    public Transform pontoDeTiro;
   
    private float proximoTiroTempo = 0f;

    void Start()
    {
        GameObject jogadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jogadorObj != null)
        {
            player = jogadorObj.transform;
        }
       
        if (agent == null) agent = GetComponent<NavMeshAgent>();
       
        agent.stoppingDistance = distanciaParaParar;

        // Desliga a rotação do NavMeshAgent para o nosso script assumir o controle
        agent.updateRotation = false;
    }

    void Update()
    {
        if (player == null) return;

        float distanciaDoJogador = Vector3.Distance(transform.position, player.position);
        agent.SetDestination(player.position);

        // Calcula a direção exata do jogador
        Vector3 direcaoParaOlhar = (player.position - transform.position).normalized;
        direcaoParaOlhar.y = 0; // Trava o eixo Y para ele não inclinar para cima/baixo
       
        if (direcaoParaOlhar != Vector3.zero)
        {
            // Aplica a rotação de forma contínua e suave
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoParaOlhar);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * velocidadeDeGiro);
        }

        // Sistema de Tiro
        if (distanciaDoJogador <= distanciaDeTiro)
        {
            if (Time.time >= proximoTiroTempo)
            {
                Atirar();
                proximoTiroTempo = Time.time + tempoEntreTiros;
            }
        }
    }

    void Atirar()
    {
        if (projetilPrefab != null && pontoDeTiro != null)
        {
            Vector3 direcaoDoTiro = (player.position - pontoDeTiro.position).normalized;
            Quaternion rotacaoDoTiro = Quaternion.LookRotation(direcaoDoTiro);
           
            Instantiate(projetilPrefab, pontoDeTiro.position, rotacaoDoTiro);
        }
    }
}