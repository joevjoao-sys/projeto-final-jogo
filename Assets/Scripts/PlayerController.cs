using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimentação Base")]
    public float speed = 15f; 
    public float gravity = -19.62f; 
    
    // NOVA VARIÁVEL PARA O PULO
    public float jumpHeight = 3f; 

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
       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. VERIFICAÇÃO DE CHÃO E GRAVIDADE
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // --- LÓGICA DO PULO ---
        // Verifica se apertou espaço (Jump) e se está no chão
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // A fórmula da física para calcular a força exata para atingir a altura desejada:
            // velocidade = raiz_quadrada(altura * -2 * gravidade)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 2. ROTAÇÃO DA CÂMERA (MOUSE LOOK)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // 3. MOVIMENTAÇÃO (WASD)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        // Aplica a gravidade ao longo do tempo e move de novo (queda livre e pulo)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}