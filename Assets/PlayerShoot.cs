using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public float range = 100f;
    public int maxAmmo = 10;
    private int currentAmmo;
    private bool isReloading = false;

    [Header("Rate of Fire")]
    public float fireRate = 0.5f;
    private float _nextFireTime = 0f;

    public LayerMask shootableMask;
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
        currentAmmo = maxAmmo;
        
        // Update ammo UI immediately
        if (UIManager.Instance != null) 
            UIManager.Instance.UpdateAmmo(currentAmmo, maxAmmo);
    }

    void Update()
    {
        if (isReloading) return;

        // Reload on R
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlayReload();
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= _nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                // Calculate time for next available shot
                _nextFireTime = Time.time + fireRate; 
            }
        }
    }

    void Shoot()
    {
        currentAmmo--;
        
        // Update UI and sound
        if (UIManager.Instance != null) UIManager.Instance.UpdateAmmo(currentAmmo, maxAmmo);
        if (AudioManager.Instance != null) AudioManager.Instance.PlayShoot();

        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, range, shootableMask))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1f);
            }

            Barrier barrier = hit.transform.GetComponent<Barrier>();
            if (barrier != null)
            {
                barrier.TakeDamage(1f); // Deal 1 damage
            }
            
            ExplosiveBarrels barrel = hit.transform.GetComponent<ExplosiveBarrels>();
            if (barrel != null)
            {
                barrel.Explode();
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(1.5f); // Reload time
        currentAmmo = maxAmmo;
        if (UIManager.Instance != null) UIManager.Instance.UpdateAmmo(currentAmmo, maxAmmo);
        isReloading = false;
    }
}