using UnityEngine;

public class AtaqueEspada : MonoBehaviour
{
    [Header("Configurações de Ataque")]
    public float alcanceAtaque = 1.5f;
    public float danoEspada = 25f;
    public Transform pontoDeAtaque;
    public LayerMask layerInimigo;

    [Header("Configurações de Knockback")]
    public float forcaKnockback = 4f; // Valores menores funcionam melhor no cinemático (ex: 3 a 5)
    public float duracaoKnockback = 0.3f; // Tempo que dura o "tranco" (0.3s é ideal)

    [Header("Animação e Cooldown")]
    public Animator animator;
    public float tempoEntreAtaques = 0.5f;
    private float proximoAtaqueTempo = 0f;

    void Update()
    {
        if (Time.time >= proximoAtaqueTempo)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                Atacar();
                proximoAtaqueTempo = Time.time + tempoEntreAtaques;
            }
        }
    }

    void Atacar()
    {
        if (animator != null)
        {
            animator.SetTrigger("Ataque");
        }

        // Detecta inimigos na área da espadada
        Collider[] inimigosAtingidos = Physics.OverlapSphere(pontoDeAtaque.position, alcanceAtaque, layerInimigo);

        foreach (Collider inimigo in inimigosAtingidos)
        {
            EnemyHealth vidaDoInimigo = inimigo.GetComponent<EnemyHealth>();
           
            if (vidaDoInimigo != null)
            {
                // 1. Aplica o Dano
                vidaDoInimigo.TakeDamage(danoEspada);

                // 2. Aplica o Knockback Cinemático! (Sem Rigidbody, sem bugs de gravidade)
                vidaDoInimigo.TomarKnockback(transform.position, forcaKnockback, duracaoKnockback);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pontoDeAtaque == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pontoDeAtaque.position, alcanceAtaque);
    }
}