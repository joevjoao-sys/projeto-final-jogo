using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Atributos da Arma")]
    public float damage = 20f;
    public float range = 100f; // Distância máxima que o tiro alcança

    [Header("Referências")]
    public Transform firePoint; // O local de onde o Raycast vai sair

    void Update()
    {
        // Input.GetButtonDown("Fire1") responde ao clique esquerdo do mouse por padrão na Unity
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            // Tenta pegar o script de vida do objeto que acabamos de acertar
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();

            // Se o objeto acertado tiver o script (ou seja, se não for uma parede ou o chão)
            if (target != null)
            {
                // Aplica o dano configurado na arma
                target.TakeDamage(damage);
            }

            // Opcional: Efeito de impacto (faísca na parede ou sangue no monstro) entraria aqui
        }

        Debug.DrawRay(firePoint.position, firePoint.forward * range, Color.red, 2f);
    }

}