using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimentação Base")]
    public float speed = 15f; // Velocidade alta para um retro-shooter
    public float gravity = -19.62f; // Gravidade um pouco mais pesada para o pulo não parecer flutuante

    [Header("Câmera e Visão")]
    public float mouseSensitivity = 300f;
    public Transform playerCamera;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
       
        // Trava e esconde o cursor do mouse no centro da tela
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. VERIFICAÇÃO DE CHÃO E GRAVIDADE
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            // Mantém o jogador levemente pressionado contra o chão
            velocity.y = -2f;
        }

        // 2. ROTAÇÃO DA CÂMERA (MOUSE LOOK)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Impede o jogador de olhar para trás quebrando o pescoço

        // Gira a câmera para cima/baixo
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Gira o corpo do jogador para os lados
        transform.Rotate(Vector3.up * mouseX);

        // 3. MOVIMENTAÇÃO (WASD)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calcula a direção baseada para onde o jogador está olhando
        Vector3 move = transform.right * x + transform.forward * z;

        // Executa o movimento
        controller.Move(move * speed * Time.deltaTime);

        // Aplica a gravidade ao longo do tempo (queda livre)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}