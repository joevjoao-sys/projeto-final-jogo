using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float distanciaColeta = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaColeta))
            {
                if (hit.collider.CompareTag("Coletavel"))
                {
                    // Aqui você chamará seu sistema de inventário para adicionar o item
                    // Por enquanto, vamos apenas destruir o item para simular a coleta
                    Debug.Log("Pegou Rum!");
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}