using UnityEngine;

public class ScrollTextura : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float velocidadeX = 0.1f;
    public float velocidadeY = 0.1f;

    private Material materialOceano;

    void Start()
    {
        materialOceano = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float deslocamentoX = Time.time * velocidadeX;
        float deslocamentoY = Time.time * velocidadeY;

        materialOceano.mainTextureOffset = new Vector2(deslocamentoX, deslocamentoY);
    }
}