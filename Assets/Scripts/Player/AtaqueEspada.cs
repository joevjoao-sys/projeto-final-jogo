using UnityEngine;

public class AtaqueEspada : MonoBehaviour
{
    [Header("Configurações de Ataque")]
    public float alcanceAtaque = 1.5f;
    public float danoEspada = 25f;
    public Transform pontoDeAtaque;
    public LayerMask layerInimigo;

    [Header("Configurações de Knockback")]
    public float forcaKnockback = 10f; // Força do empurrão. Ajuste no Inspector!

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

        // --- CÓDIGO PARA JOGOS 3D ---
        Collider[] inimigosAtingidos = Physics.OverlapSphere(pontoDeAtaque.position, alcanceAtaque, layerInimigo);

        foreach (Collider inimigo in inimigosAtingidos)
        {
            // 1. Aplica o Dano
            EnemyHealth vidaDoInimigo = inimigo.GetComponent<EnemyHealth>();
            if (vidaDoInimigo != null)
            {
                vidaDoInimigo.TakeDamage(danoEspada);
            }

            // 2. Aplica o Knockback (Física 3D)
            Rigidbody rbInimigo = inimigo.GetComponent<Rigidbody>();
            if (rbInimigo != null)
            {
                // Calcula a direção do empurrão (Posição do Inimigo menos a Posição da Espada)
                Vector3 direcao = (inimigo.transform.position - transform.position).normalized;
               
                // Mantém o empurrão apenas na horizontal (opcional, evita que o inimigo voe para o céu)
                direcao.y = 0.2f;

                // Aplica a força instantaneamente usando Impulse
                rbInimigo.AddForce(direcao * forcaKnockback, ForceMode.VelocityChange);
            }
        }

        /* --- SE SEU JOGO FOR 2D, APAGUE O BLOCO 3D ACIMA E USE ESTE ABAIXO: ---
       
        Collider2D[] inimigosAtingidos = Physics2D.OverlapCircleAll(pontoDeAtaque.position, alcanceAtaque, layerInimigo);

        foreach (Collider2D inimigo in inimigosAtingidos)
        {
            EnemyHealth vidaDoInimigo = inimigo.GetComponent<EnemyHealth>();
            if (vidaDoInimigo != null)
            {
                vidaDoInimigo.TakeDamage(danoEspada);
            }

            Rigidbody2D rbInimigo = inimigo.GetComponent<Rigidbody2D>();
            if (rbInimigo != null)
            {
                Vector2 direcao = (inimigo.transform.position - transform.position).normalized;
               
                // Zera a velocidade atual para o knockback não ser cancelado se ele estiver andando
                rbInimigo.linearVelocity = Vector2.zero;
               
                rbInimigo.AddForce(direcao * forcaKnockback, ForceMode2D.Impulse);
            }
        }
        ----------------------------------------------------------------------- */
    }

    void OnDrawGizmosSelected()
    {
        if (pontoDeAtaque == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pontoDeAtaque.position, alcanceAtaque);
    }
}

