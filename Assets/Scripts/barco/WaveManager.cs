using UnityEngine;
using System.Collections;
using TMPro; // Obrigatório para manipular textos do TextMeshPro

public class WaveManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject enemyRangedPrefab;
    public GameObject puddlePrefab; 
    
    [Header("Listas de Pontos (Arraste aqui)")]
    public Transform[] deckSpawnPoints;
    public Transform[] edgeSpawnPoints;

    [Header("Interface (UI)")]
    public TextMeshProUGUI textoAvisoWave; // Arraste o seu TextoAvisoWave aqui

    [Header("Controle da Horda")]
    public int waveAtual = 1;
    public int waveMaxima = 10; // Fim de jogo após essa wave
    public int inimigosNestaWave = 3;
    
    private int inimigosVivos = 0;
    private bool hordaRolando = false;

    void Start()
    {
        // Garante que o texto comece apagado
        if (textoAvisoWave != null) textoAvisoWave.gameObject.SetActive(false);
        
        StartCoroutine(IniciarProximaWave());
    }

    void Update()
    {
        if (hordaRolando && inimigosVivos <= 0)
        {
            hordaRolando = false;
            
            if (waveAtual >= waveMaxima)
            {
                // Se terminou a Wave 10, avisa o GameManager para mostrar os créditos
                GameManager gm = FindObjectOfType<GameManager>();
                if (gm != null)
                {
                    gm.MostrarCreditos();
                }
            }
            else
            {
                // Prepara a próxima wave
                waveAtual++;
                inimigosNestaWave += 2; 
                StartCoroutine(IniciarProximaWave());
            }
        }
    }

    IEnumerator IniciarProximaWave()
    {
        if (textoAvisoWave != null)
        {
            textoAvisoWave.gameObject.SetActive(true);
            
            // Loop da contagem regressiva de 3 a 1
            for (int i = 3; i > 0; i--)
            {
                textoAvisoWave.text = "A ONDA " + waveAtual + " COMEÇA EM " + i + "...";
                yield return new WaitForSeconds(1f);
            }
            
            textoAvisoWave.text = "MATE TODOS!";
            yield return new WaitForSeconds(1.5f);
            textoAvisoWave.gameObject.SetActive(false); // Esconde o texto
        }
        else
        {
            // Fallback de 3 segundos se você não configurar o texto na Unity
            yield return new WaitForSeconds(3f); 
        }
        
        for (int i = 0; i < inimigosNestaWave; i++)
        {
            StartCoroutine(SpawnarUmMonstro());
            yield return new WaitForSeconds(1.5f); 
        }

        hordaRolando = true; 
    }

    IEnumerator SpawnarUmMonstro()
    {
        bool nascerNoConves = (Random.value > 0.5f);
        Transform pontoEscolhido;

        if (nascerNoConves && deckSpawnPoints.Length > 0)
        {
            pontoEscolhido = deckSpawnPoints[Random.Range(0, deckSpawnPoints.Length)];
            Instantiate(puddlePrefab, pontoEscolhido.position, Quaternion.identity);
        }
        else if (edgeSpawnPoints.Length > 0)
        {
            pontoEscolhido = edgeSpawnPoints[Random.Range(0, edgeSpawnPoints.Length)];
        }
        else
        {
            yield break; 
        }

        inimigosVivos++; 

        yield return new WaitForSeconds(1f);

        GameObject prefabParaCriar = enemyPrefab; 
        
        if (waveAtual >= 3 && enemyRangedPrefab != null)
        {
            if (Random.value < 0.35f)
            {
                prefabParaCriar = enemyRangedPrefab;
            }
        }

        Instantiate(prefabParaCriar, pontoEscolhido.position, pontoEscolhido.rotation);
    }

    public void MonstroMorreu()
    {
        inimigosVivos--;
    }
}