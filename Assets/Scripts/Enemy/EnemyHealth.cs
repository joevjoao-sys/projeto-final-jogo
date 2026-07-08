using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    public GameObject pimentaPrefab; 
    
    [Range(0f, 100f)]
    public float chanceDeDrop = 40f;

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
        
        WaveManager gerenciadorDeHorda = FindObjectOfType<WaveManager>();
        if (gerenciadorDeHorda != null)
        {
            gerenciadorDeHorda.MonstroMorreu();
        }
        
        Destroy(gameObject);
    }

    private void GerarDrop()
    {
        float sorteio = Random.Range(0f, 100f);
        
        if (sorteio <= chanceDeDrop)
        {
            GameObject itemSorteado = null;
            float qualItem = Random.Range(0f, 100f);

            if (qualItem <= 20f) 
            {
                itemSorteado = pimentaPrefab;
            }
            else if (qualItem <= 60f) 
            {
                itemSorteado = rumPrefab;
            }
            else 
            {
                itemSorteado = municaoPrefab;
            }

            if (itemSorteado != null)
            {
                Instantiate(itemSorteado, transform.position + Vector3.up, Quaternion.identity);
            }
        }
    }

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

        Vector3 direcao = (transform.position - origemDoAtaque).normalized;
        direcao.y = 0;
        
        Vector3 posFinal = transform.position + (direcao * forca);

        if (Physics.Raycast(transform.position, direcao, out RaycastHit hit, forca))
        {
            posFinal = hit.point - (direcao * 0.5f);
        }

        while (tempoDecorrido < duracao)
        {
            tempoDecorrido += Time.deltaTime;
            float t = tempoDecorrido / duracao;
            transform.position = Vector3.Lerp(posInicial, posFinal, t * (2f - t));
            yield return null;
        }

        sofrendoKnockback = false;
    }
}