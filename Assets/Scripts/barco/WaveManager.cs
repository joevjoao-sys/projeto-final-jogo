using UnityEngine;
using System.Collections;
using TMPro; // <-- BIBLIOTECA NOVA OBRIGATÓRIA PARA A FONTE

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
    
    [Header("Interface")]
    // --- NOVO: Variável para o seu texto na tela ---
    public TextMeshProUGUI textoAvisoWave; 

    private int inimigosVivos = 0;
    private bool hordaRolando = false;

    void Start()
    {
        // Garante que o texto comece desligado quando o jogo abre
        if (textoAvisoWave != null) textoAvisoWave.gameObject.SetActive(false);
        
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
        
        // --- NOVO: Chama a animação do letreiro ---
        if (textoAvisoWave != null)
        {
            StartCoroutine(AnimarTextoWave("WAVE " + waveAtual));
        }

        yield return new WaitForSeconds(3f);
        
        for (int i = 0; i < inimigosNestaWave; i++)
        {
            StartCoroutine(SpawnarUmMonstro());
            yield return new WaitForSeconds(1.5f);
        }

        hordaRolando = true;
    }

    // --- NOVA ROTINA: O EFEITO DO TEXTO ---
    IEnumerator AnimarTextoWave(string mensagem)
    {
        textoAvisoWave.text = mensagem;
        textoAvisoWave.gameObject.SetActive(true);
        
        // Garante que a cor comece 100% visível
        Color cor = textoAvisoWave.color;
        cor.a = 1f;
        textoAvisoWave.color = cor;

        // 1. Efeito POP (Cresce de 0.1 até 1.2 bem rápido)
        float tempo = 0;
        while (tempo < 0.3f)
        {
            tempo += Time.deltaTime;
            float escala = Mathf.Lerp(0.1f, 1.2f, tempo / 0.3f);
            textoAvisoWave.transform.localScale = new Vector3(escala, escala, 1f);
            yield return null;
        }
        
        // 2. Dá uma assentada no tamanho original
        textoAvisoWave.transform.localScale = Vector3.one;

        // 3. Fica paradão por 1.5 segundos para o jogador ler
        yield return new WaitForSeconds(1.5f);

        // 4. Efeito FADE OUT (Fica transparente enquanto diminui)
        tempo = 0;
        while (tempo < 1f)
        {
            tempo += Time.deltaTime;
            
            // Diminui o Alpha (transparência)
            cor.a = Mathf.Lerp(1f, 0f, tempo / 1f);
            textoAvisoWave.color = cor;
            
            // Diminui o tamanho
            float escala = Mathf.Lerp(1f, 0.5f, tempo / 1f);
            textoAvisoWave.transform.localScale = new Vector3(escala, escala, 1f);
            
            yield return null;
        }

        // Desliga o objeto pra não gastar processamento
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
        Debug.Log("Monstro morto. Restam: " + inimigosVivos);
    }
}