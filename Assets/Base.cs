using UnityEngine;

public class Base : MonoBehaviour
{
    public float maxHealth = 99f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        // Update UI on start
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(33f); // Base takes damage
            if (SpawnManager.Instance != null) SpawnManager.Instance.OnEnemyKilled();

            other.gameObject.SetActive(false);
        }
    }

    void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (AudioManager.Instance != null) AudioManager.Instance.PlayBaseHit();

        // Red flash
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
            UIManager.Instance.ShowDamageFlash();
        }

        if (currentHealth <= 1f)
        {
            currentHealth = 0f;
            Debug.Log("GAME OVER");
            UIManager.Instance.ShowLose();
        }
    }
}