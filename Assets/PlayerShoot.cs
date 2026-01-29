using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public LayerMask hitLayers;
    public float range = 100f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 rayOrigin = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, transform.forward, out hit, range, hitLayers))
        {
            Debug.Log("Попал в: " + hit.collider.name);

            Barrier barrier = hit.transform.GetComponent<Barrier>();
            if (barrier != null)
            {
                barrier.TakeDamage(1f);
            }

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DeathState();
            }
        }
    }
}