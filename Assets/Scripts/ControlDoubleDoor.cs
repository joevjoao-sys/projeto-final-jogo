using UnityEngine;

public class ControlDoubleDoor : MonoBehaviour
{
    public GameObject portaEsquerda;
    public GameObject portaDireita;
    public float angulo = 90f;
    public float velocidade = 4f;

    private bool perto = false;
    private bool aberta = false;
    private Quaternion rotInicialEsq;
    private Quaternion rotInicialDir;
   
    private Transform jogador; // Guarda a posição do pirata
    private float direcaoAbertura = 1f; // Decide se abre pra frente ou pra trás

    void Start() {
        if(portaEsquerda) rotInicialEsq = portaEsquerda.transform.localRotation;
        if(portaDireita) rotInicialDir = portaDireita.transform.localRotation;
    }

    void Update() {
        if (perto && Input.GetKeyDown(KeyCode.E)) {
            // Só calcula a direção se a porta estiver fechada indo abrir
            if (!aberta) {
                // Calcula de que lado do cubo invisível o jogador está
                Vector3 posicaoRelativa = jogador.position - transform.position;
                float lado = Vector3.Dot(transform.forward, posicaoRelativa);

                // Se estiver de um lado abre positivo, se estiver do outro abre negativo
                if (lado > 0) direcaoAbertura = 1f;
                else direcaoAbertura = -1f;
            }
           
            aberta = !aberta;
        }

        // Aplica o movimento na porta Esquerda
        if(portaEsquerda) {
            Quaternion alvoEsq = aberta ? rotInicialEsq * Quaternion.Euler(0, angulo * direcaoAbertura, 0) : rotInicialEsq;
            portaEsquerda.transform.localRotation = Quaternion.Slerp(portaEsquerda.transform.localRotation, alvoEsq, Time.deltaTime * velocidade);
        }
       
        // Aplica o movimento na porta Direita (sempre o inverso da esquerda)
        if(portaDireita) {
            Quaternion alvoDir = aberta ? rotInicialDir * Quaternion.Euler(0, -angulo * direcaoAbertura, 0) : rotInicialDir;
            portaDireita.transform.localRotation = Quaternion.Slerp(portaDireita.transform.localRotation, alvoDir, Time.deltaTime * velocidade);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            perto = true;
            jogador = other.transform; // O script "olha" pro jogador quando ele entra na área
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) perto = false;
    }
}