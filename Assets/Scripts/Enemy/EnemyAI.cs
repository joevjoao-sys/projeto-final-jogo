using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Sistema de Visão e Perseguição")]
    public float raioDeVisao = 30f; // Campo de visão BEM maior (ajuste no Inspector)
    public float distanciaDeAtaque = 2.5f; // Quão perto ele precisa chegar para bater
   
    [Header("Combate")]
    public float dano = 15f;
    public float tempoEntreAtaques = 1.5f;

    private Transform player;
    private NavMeshAgent agent;
    private float tempoDoUltimoAtaque;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
       
        // Acha o jogador automaticamente
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        // Se o jogador não existir, o monstro fica quieto
        if (player == null) return;

        // Calcula a distância real em 3D
        float distanciaDoJogador = Vector3.Distance(transform.position, player.position);

        // 1. O Jogador está dentro da visão do monstro?
        if (distanciaDoJogador <= raioDeVisao)
        {
            // 2. Está longe o suficiente para precisar correr atrás?
            if (distanciaDoJogador > distanciaDeAtaque)
            {
                agent.isStopped = false; // Começa a correr
                agent.SetDestination(player.position); // Vai para onde o jogador está
            }
            // 3. Chegou perto o suficiente? Então ataca!
            else
            {
                agent.isStopped = true; // Para de correr para bater
                AtacarJogador();
            }
        }
        else
        {
            // Se o jogador conseguir fugir do raio gigante, o monstro desiste e para.
            agent.isStopped = true;
        }
    }

    void AtacarJogador()
    {
        // Verifica se já deu tempo de dar o próximo golpe
        if (Time.time >= tempoDoUltimoAtaque + tempoEntreAtaques)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(dano);
                Debug.Log("O Monstro te acertou um golpe!");
            }
           
            tempoDoUltimoAtaque = Time.time; // Reseta o cronômetro do ataque
        }
    }

    // --- A MÁGICA VISUAL ---
    // Isso desenha as linhas de visão na aba Scene para você debugar!
    void OnDrawGizmosSelected()
    {
        // Bola amarela = Até onde ele enxerga
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeVisao);

        // Bola vermelha = Área de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaDeAtaque);
    }
}