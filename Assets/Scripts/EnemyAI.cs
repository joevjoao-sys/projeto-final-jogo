using UnityEngine;
using UnityEngine.AI; // Necessário para usar o NavMeshAgent

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Configurações de Combate")]
    public float attackRange = 2.5f; // Distância para começar a bater
    public float damage = 15f;
    public float attackCooldown = 1.5f; // Tempo entre cada ataque

    private Transform playerTransform;
    private NavMeshAgent agent;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
       
        // O monstro procura automaticamente quem tem a tag "Player" na cena
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    void Update()
    {
        // Se o jogador não existir (ou já tiver morrido), não faz nada
        if (playerTransform == null) return;

        // Calcula a distância entre o monstro e o jogador
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // ESTADO: PERSEGUIR (Chase)
        if (distanceToPlayer > attackRange)
        {
            agent.isStopped = false; // Permite que ele ande
            agent.SetDestination(playerTransform.position); // Vai atrás do jogador
        }
        // ESTADO: ATACAR (Attack)
        else
        {
            agent.isStopped = true; // Para de andar para focar no ataque
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        // Verifica se já passou tempo suficiente desde o último ataque
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Tenta pegar o script de vida do jogador
            PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
           
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Monstro atacou o Capitão causando " + damage + " de dano!");
            }
           
            lastAttackTime = Time.time; // Reseta o cronômetro do ataque
        }
    }
}