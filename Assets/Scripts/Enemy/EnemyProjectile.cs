using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Configurações da Bala")]
    public float velocidade = 15f;
    public float dano = 35f;
    public float tempoDeVida = 5f;

    [Header("Efeito de Perseguição (Teleguiado)")]
    [Tooltip("Quanto maior o número, mais fechada é a curva que a bala faz.")]
    public float forcaDoGiro = 50f;
   
    private Transform alvoJogador;

    void Start()
    {
        // Encontra o jogador na cena pela Tag
        GameObject jogadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jogadorObj != null)
        {
            alvoJogador = jogadorObj.transform;
        }

        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        // Se o jogador estiver vivo, a bala gira suavemente na direção dele
        if (alvoJogador != null)
        {
            // Aponta para o centro/peito do jogador (subindo um pouco no eixo Y)
            Vector3 posicaoAlvo = alvoJogador.position + Vector3.up * 1f;
            Vector3 direcaoParaAlvo = posicaoAlvo - transform.position;
           
            if (direcaoParaAlvo != Vector3.zero)
            {
                // Variável declarada corretamente aqui
                Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoParaAlvo);
               
                // Faz a curva suave com base no tempo e na força configurada (corrigido aqui!)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacaoAlvo, forcaDoGiro * Time.deltaTime);
            }
        }

        // Move a bala sempre para frente (seguindo para onde ela está girando)
        transform.Translate(Vector3.forward * velocidade * Time.deltaTime);
    }

    void OnTriggerEnter(Collider outro)
    {
        if (outro.CompareTag("Player"))
        {
            PlayerHealth vidaJogador = outro.GetComponent<PlayerHealth>();
            if (vidaJogador != null)
            {
                vidaJogador.TakeDamage(dano);
            }
            Destroy(gameObject);
        }
        else if (!outro.CompareTag("Enemy") && !outro.CompareTag("Coletavel"))
        {
            Destroy(gameObject);
        }
    }
}