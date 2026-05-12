using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour
{
    [Header("Sistema de Visão e Perseguição")]
    public float raioDeVisao = 35f;
    public float distanciaDeAtaque = 3f;
    public float velocidadeVoo = 8f;
    public float velocidadeRotacao = 5f;
    public float alturaDesejada = 4f; // Altura que ela tenta manter

    [Header("Combate")]
    public float dano = 10f;
    public float tempoEntreAtaques = 2f;

    private Transform player;
    private float tempoDoUltimoAtaque;

    void Start()
    {
        // Acha o jogador automaticamente
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanciaDoJogador = Vector3.Distance(transform.position, player.position);

        if (distanciaDoJogador <= raioDeVisao)
        {
            if (distanciaDoJogador > distanciaDeAtaque)
            {
                PerseguirJogador();
            }
            else
            {
                AtacarJogador();
            }
        }
    }

    void PerseguirJogador()
    {
        // Define o alvo: a posição do jogador + a altura desejada
        Vector3 posicaoAlvo = player.position + Vector3.up * alturaDesejada;

        // Move a arara em direção ao alvo
        transform.position = Vector3.MoveTowards(transform.position, posicaoAlvo, velocidadeVoo * Time.deltaTime);

        // Rotaciona suavemente para olhar para o jogador
        Vector3 direcao = (player.position - transform.position).normalized;
        if (direcao != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direcao);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * velocidadeRotacao);
        }
    }

    void AtacarJogador()
    {
        // Dá um "rasante" no ataque
        transform.position = Vector3.MoveTowards(transform.position, player.position, velocidadeVoo * Time.deltaTime);

        if (Time.time >= tempoDoUltimoAtaque + tempoEntreAtaques)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(dano);
                Debug.Log("A Arara te deu uma bicada!");
            }
            tempoDoUltimoAtaque = Time.time;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeVisao);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaDeAtaque);
    }
}