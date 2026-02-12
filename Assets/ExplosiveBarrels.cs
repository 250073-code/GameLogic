using UnityEngine;

public class ExplosiveBarrels : MonoBehaviour
{
    [Header("Settings")]
    public GameObject explosionEffect;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;

    public void Explode()
    {
        // 1. Create explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            AudioManager.Instance?.PlayExplosion();
        }

        // 2. Find all enemies and physics objects within radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Deal heavy damage to ensure enemy dies
                enemy.TakeDamage(10f);
            }

            // If you want bodies or other objects to scatter
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }
}