using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour
{
    public float maxHealth = 5f;
    private float _currentHealth;
    public float rechargeDelay = 5f;

    private MeshRenderer _renderer;
    private Collider _collider;
    private Color _originalColor;

    void Start()
    {
        _currentHealth = maxHealth;
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        
        _originalColor = _renderer.material.color;
    }

    public void TakeDamage(float amount)
    {
        if (_currentHealth <= 0) return;

        _currentHealth -= amount;
        
        float healthPercent = _currentHealth / maxHealth;
        _renderer.material.color = Color.Lerp(Color.red, _originalColor, healthPercent);

        if (_currentHealth <= 0)
        {
            StartCoroutine(RechargeRoutine());
        }
    }

    IEnumerator RechargeRoutine()
    {
        _renderer.enabled = false;
        _collider.enabled = false;

        yield return new WaitForSeconds(rechargeDelay);

        _currentHealth = maxHealth;
        _renderer.material.color = _originalColor;
        _renderer.enabled = true;
        _collider.enabled = true;
    }
}