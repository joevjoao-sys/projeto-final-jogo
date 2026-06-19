using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Necessário para usar a Coroutine do Knockback

public class EnemyHealth : MonoBehaviour
{
    [Header("Saúde do Monstro")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Interface")]
    public Image barraDeVida;
    public Transform canvasDaVida;
    private Camera mainCamera;

    [Header("Configurações de Drop")]
    public GameObject rumPrefab;
    public GameObject municaoPrefab;
    [Range(0f, 100f)]
    public float chanceDeDrop = 40f;

    // Controle do Knockback
    private bool sofrendoKnockback = false;

    void Start()
    {
        currentHealth = maxHealth;
        mainCamera = Camera.main;

        AtualizarBarraDeVida();
    }

    void LateUpdate()
    {
        if (canvasDaVida != null && mainCamera != null)
        {
            canvasDaVida.LookAt(canvasDaVida.position + mainCamera.transform.forward);
        }
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        Debug.Log($"{gameObject.name} tomou um golpe! Vida: {currentHealth}");

        AtualizarBarraDeVida();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void AtualizarBarraDeVida()
    {
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        GerarDrop();
       
        Debug.Log($"{gameObject.name} foi pro fundo do mar!");
        Destroy(gameObject);
    }

    private void GerarDrop()
    {
        float sorteio = Random.Range(0f, 100f);
       
        if (sorteio <= chanceDeDrop)
        {
            GameObject itemSorteado = (Random.value > 0.5f) ? rumPrefab : municaoPrefab;

            if (itemSorteado != null)
            {
                Instantiate(itemSorteado, transform.position + Vector3.up, Quaternion.identity);
                Debug.Log($"Saque à vista! Dropou {itemSorteado.name}");
            }
        }
    }

    // --- FUNÇÕES DE KNOCKBACK ADICIONADAS AQUI ---

    public void TomarKnockback(Vector3 origemDoAtaque, float forcaKnockback, float duracao)
    {
        if (!sofrendoKnockback)
        {
            StartCoroutine(RotinaDeKnockback(origemDoAtaque, forcaKnockback, duracao));
        }
    }

    private IEnumerator RotinaDeKnockback(Vector3 origemDoAtaque, float forca, float duracao)
    {
        sofrendoKnockback = true;

        float tempoDecorrido = 0f;
        Vector3 posInicial = transform.position;

        // Calcula a direção para trás (ignorando o eixo Y)
        Vector3 direcao = (transform.position - origemDoAtaque).normalized;
        direcao.y = 0;
       
        Vector3 posFinal = transform.position + (direcao * forca);

        // Checa se tem uma parede atrás para não atravessar o mapa
        if (Physics.Raycast(transform.position, direcao, out RaycastHit hit, forca))
        {
            posFinal = hit.point - (direcao * 0.5f);
        }

        while (tempoDecorrido < duracao)
        {
            tempoDecorrido += Time.deltaTime;
           
            // Ease-Out: Rápido no início, freia no final
            float t = tempoDecorrido / duracao;
            float transicaoSuave = t * (2f - t);

            transform.position = Vector3.Lerp(posInicial, posFinal, transicaoSuave);

            yield return null;
        }

        sofrendoKnockback = false;
    }
}