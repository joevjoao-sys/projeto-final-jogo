using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Atributos da Arma")]
    public float damage = 20f;
    public float range = 100f;

    [Header("Referências")]
    public Transform firePoint;
    public GameObject trailPrefab; // Arraste o BulletTrail_Prefab aqui

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        // Definimos onde o tiro termina (no alvo ou no alcance máximo)
        Vector3 endPoint = firePoint.position + firePoint.forward * range;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            endPoint = hit.point; // Tiro bateu em algo

            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        // Criar o rastro visual
        CreateTrail(endPoint);
    }

    void CreateTrail(Vector3 targetPosition)
    {
        // Instancia o rastro
        GameObject trailObj = Instantiate(trailPrefab, firePoint.position, Quaternion.identity);
        LineRenderer lr = trailObj.GetComponent<LineRenderer>();

        if (lr != null)
        {
            // Define o início (ponta da arma) e fim (onde o raycast bateu)
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, targetPosition);
        }
    }
}