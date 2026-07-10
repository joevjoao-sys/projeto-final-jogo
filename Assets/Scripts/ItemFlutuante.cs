using UnityEngine;

public class ItemFlutuante : MonoBehaviour
{
    [Header("Configurações do Efeito")]
    public float velocidadeDeGiro = 100f; // Velocidade da rotação
    public float alturaDoPulo = 0.3f;     // O quanto ele sobe e desce
    public float velocidadeDaOnda = 3f;   // Rapidez do movimento de subir/descer

    private Vector3 posicaoInicial;

    void Start()
    {
        // Salva a posição exata de onde o monstro dropou o item no chão
        posicaoInicial = transform.position;
    }

    void Update()
    {
        // 1. Gira o item no eixo Y
        transform.Rotate(Vector3.up, velocidadeDeGiro * Time.deltaTime, Space.World);

        // 2. Faz o item subir e descer usando uma curva de seno
        float novaPosicaoY = posicaoInicial.y + (Mathf.Sin(Time.time * velocidadeDaOnda) * alturaDoPulo);
        
        // Aplica a nova posição mantendo o X e Z originais
        transform.position = new Vector3(transform.position.x, novaPosicaoY, transform.position.z);
    }
}