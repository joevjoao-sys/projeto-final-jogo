using UnityEngine;
using System.Collections;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;      
    public GameObject enemyRangedPrefab; 
    public GameObject puddlePrefab;
    
    [Header("Listas de Pontos (Arraste aqui)")]
    public Transform[] deckSpawnPoints;
    public Transform[] edgeSpawnPoints;

    [Header("Controle da Horda")]
    public int waveAtual = 1;
    public int inimigosNestaWave = 3;
    
    [Tooltip("Quantos segundos o Capitão tem para recolher o loot do chão antes da próxima onda")]
    public float tempoDeDescanso = 6f; 
    
    [Header("Interface")]
    public TextMeshProUGUI textoAvisoWave; 
    
    private bool hordaRolando = false;

    void Start()
    {
        if (textoAvisoWave != null) textoAvisoWave.gameObject.SetActive(false);
        
        // A Wave 1 começa rápido, com apenas 3 segundos de espera inicial
        StartCoroutine(IniciarProximaWave(3f));
    }

    void Update()
    {
        // Só começa a escanear a morte dos monstros DEPOIS que todos da onda atual já nasceram
        if (hordaRolando)
        {
            // SISTEMA À PROVA DE BUGS: 
            // O código escaneia o mapa e procura qualquer objeto que tenha vida de inimigo.
            EnemyHealth[] todosOsInimigosNoMapa = FindObjectsOfType<EnemyHealth>();

            // Se o escaneamento retornar ZERO, significa que a área está totalmente limpa
            if (todosOsInimigosNoMapa.Length == 0)
            {
                hordaRolando = false;
                waveAtual++;
                inimigosNestaWave += 2; // Aumenta a dificuldade para a próxima
                
                // Dispara a próxima onda respeitando o tempo de descanso para você pegar os itens
                StartCoroutine(IniciarProximaWave(tempoDeDescanso));
            }
        }
    }

    IEnumerator IniciarProximaWave(float espera)
    {
        Debug.Log($"Área limpa! A onda {waveAtual} começa em {espera} segundos!");
        
        // Pausa dramática para o jogador respirar e coletar os drops do chão
        yield return new WaitForSeconds(espera);
        
        if (textoAvisoWave != null)
        {
            StartCoroutine(AnimarTextoWave("WAVE " + waveAtual));
        }

        // Tempo para o letreiro aparecer e sumir antes de nascer bicho
        yield return new WaitForSeconds(2f);
        
        for (int i = 0; i < inimigosNestaWave; i++)
        {
            StartCoroutine(SpawnarUmMonstro());
            yield return new WaitForSeconds(1.5f); // Intervalo entre o nascimento de cada bicho
        }

        // Só libera o Update para verificar a morte deles depois que todos terminaram de nascer
        hordaRolando = true; 
    }

    IEnumerator AnimarTextoWave(string mensagem)
    {
        textoAvisoWave.text = mensagem;
        textoAvisoWave.gameObject.SetActive(true);
        
        Color cor = textoAvisoWave.color;
        cor.a = 1f;
        textoAvisoWave.color = cor;

        float tempo = 0;
        while (tempo < 0.3f)
        {
            tempo += Time.deltaTime;
            float escala = Mathf.Lerp(0.1f, 1.2f, tempo / 0.3f);
            textoAvisoWave.transform.localScale = new Vector3(escala, escala, 1f);
            yield return null;
        }
        
        textoAvisoWave.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(1.5f);

        tempo = 0;
        while (tempo < 1f)
        {
            tempo += Time.deltaTime;
            cor.a = Mathf.Lerp(1f, 0f, tempo / 1f);
            textoAvisoWave.color = cor;
            float escala = Mathf.Lerp(1f, 0.5f, tempo / 1f);
            textoAvisoWave.transform.localScale = new Vector3(escala, escala, 1f);
            yield return null;
        }

        textoAvisoWave.gameObject.SetActive(false);
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

        yield return new WaitForSeconds(1f);

        GameObject prefabParaCriar = enemyPrefab; 

        if (waveAtual >= 3 && enemyRangedPrefab != null)
        {
            if (Random.value < 0.35f) // 35% de chance de ser o Atirador nas ondas mais altas
            {
                prefabParaCriar = enemyRangedPrefab;
            }
        }

        Instantiate(prefabParaCriar, pontoEscolhido.position, pontoEscolhido.rotation);
    }

    // Deixei essa função aqui em branco apenas para o seu inimigo não dar erro de script,
    // já que agora o gerenciador rastreia o mapa todo de forma automática.
    public void MonstroMorreu()
    {
        
    }
}