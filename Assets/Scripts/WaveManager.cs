using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject puddlePrefab; 
    
    [Header("Listas de Pontos (Arraste aqui)")]
    public Transform[] deckSpawnPoints; // Pontos do meio do barco
    public Transform[] edgeSpawnPoints; // Pontos das bordas

    [Header("Controle da Horda")]
    public int waveAtual = 1;
    public int inimigosNestaWave = 3;
    
    private int inimigosVivos = 0;
    private bool hordaRolando = false;

    void Start()
    {
        // Inicia a primeira onda
        StartCoroutine(IniciarProximaWave());
    }

    void Update()
    {
        // Se a horda começou e todos morreram, passa para a próxima!
        if (hordaRolando && inimigosVivos <= 0)
        {
            hordaRolando = false;
            waveAtual++;
            inimigosNestaWave += 2; // Aumenta 2 monstros por onda
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
            yield return new WaitForSeconds(1.5f); // Tempo de respiro entre o nascimento de cada monstro
        }

        hordaRolando = true; // Avisa que todos os monstros da onda já nasceram
    }

    IEnumerator SpawnarUmMonstro()
    {
        // Sorteia se vai nascer no meio do barco (convés) ou na borda
        bool nascerNoConves = (Random.value > 0.5f);

        Transform pontoEscolhido;

        if (nascerNoConves && deckSpawnPoints.Length > 0)
        {
            // Pega um ponto aleatório do convés
            pontoEscolhido = deckSpawnPoints[Random.Range(0, deckSpawnPoints.Length)];
            // Cria a poça visual
            Instantiate(puddlePrefab, pontoEscolhido.position, Quaternion.identity);
        }
        else if (edgeSpawnPoints.Length > 0)
        {
            // Pega um ponto aleatório da borda
            pontoEscolhido = edgeSpawnPoints[Random.Range(0, edgeSpawnPoints.Length)];
        }
        else
        {
            yield break; // Prevenção de erro se esquecer de colocar os pontos na Unity
        }

        // Espera 1 segundo para dar tempo do jogador ver a poça e se afastar
        yield return new WaitForSeconds(1f);

        // Cria o inimigo de fato!
        Instantiate(enemyPrefab, pontoEscolhido.position, pontoEscolhido.rotation);
        inimigosVivos++;
    }

    // O Inimigo vai chamar essa função quando morrer
    public void MonstroMorreu()
    {
        inimigosVivos--;
    }
}