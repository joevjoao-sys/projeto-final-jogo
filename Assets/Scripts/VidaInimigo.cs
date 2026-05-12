using UnityEngine;
using UnityEngine.UI; // Necessário para controlar a interface

public class VidaInimigo : MonoBehaviour
{
    [Header("Status")]
    public float vidaMaxima = 100f;
    private float vidaAtual;

    [Header("Interface")]
    public Image barraDeVida; // Arraste a imagem que você configurou como "Filled" aqui
    public Transform canvasDaVida; // Arraste o Canvas da vida do inimigo aqui

    void Start()
    {
        vidaAtual = vidaMaxima;
        barraDeVida.fillAmount = 1f;
    }

    void Update()
    {
        // Faz o Canvas sempre olhar para a câmera do jogador
        if (canvasDaVida != null)
        {
            canvasDaVida.LookAt(canvasDaVida.position + Camera.main.transform.rotation * Vector3.forward,
                                Camera.main.transform.rotation * Vector3.up);
        }
    }

    // Chame esta função pelo script do seu tiro quando acertar o inimigo
    public void ReceberDano(float dano)
    {
        vidaAtual -= dano;

        if (vidaAtual < 0)
            vidaAtual = 0;

        // Atualiza a barra (fillAmount exige um valor entre 0 e 1)
        barraDeVida.fillAmount = vidaAtual / vidaMaxima;

        if (vidaAtual == 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        // Insira aqui a lógica de morte (ex: tocar animação ou destruir o objeto)
        Destroy(gameObject);
    }
}