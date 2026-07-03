using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;      // Seu inimigo padrão (corpo a corpo)
    public GameObject enemyRangedPrefab; // Arraste seu NOVO prefab de inimigo atirador aqui!
    public GameObject puddlePrefab;
   
    [Header("Listas de Pontos (Arraste aqui)")]
    public Transform[] deckSpawnPoints;
    public Transform[] edgeSpawnPoints;

    [Header("Controle da Horda")]
    public int waveAtual = 1;
    public int inimigosNestaWave = 3;
   
    private int inimigosVivos = 0;
    private bool hordaRolando = false;

    void Start()
    {
        StartCoroutine(IniciarProximaWave());
    }

    void Update()
    {
        if (hordaRolando && inimigosVivos <= 0)
        {
            hordaRolando = false;
            waveAtual++;
            inimigosNestaWave += 2;
            StartCoroutine(IniciarProximaWave());
        }
    }

    IEnumerator IniciarProximaWave()
    {
        Debug.Log("Prepare-se! A Onda " + waveAtual + " vai começar em 3 segundos!");
        yield return new WaitForSeconds(3f);
       
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

        // --- LÓGICA DE SORTEIO MODIFICADA AQUI ---
        GameObject prefabParaCriar = enemyPrefab; // O padrão sempre será o corpo a corpo

        // Só cogita colocar o atirador se a wave for MAIOR ou IGUAL a 3
        if (waveAtual >= 3 && enemyRangedPrefab != null)
        {
            // 35% de chance de nascer o atirador, 65% de nascer o corpo a corpo normal
            if (Random.value < 0.35f)
            {
                prefabParaCriar = enemyRangedPrefab;
            }
        }

        // Cria o inimigo sorteado no ponto correto
        Instantiate(prefabParaCriar, pontoEscolhido.position, pontoEscolhido.rotation);
    }

    public void MonstroMorreu()
    {
        inimigosVivos--;
        Debug.Log("Monstro morto. Restam: " + inimigosVivos);
    }
}