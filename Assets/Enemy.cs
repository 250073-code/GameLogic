using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _anim;
    private Rigidbody _rb;
    
    private bool _isDead = false;
    private bool _finishedHiding = false;
    private Transform _selectedHidePoint;

    public float health = 3f;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // Full reset for pool
        _isDead = false;
        _finishedHiding = false;
        health = 3f;

        if (_agent != null)
        {
            _agent.enabled = true;
            _agent.speed = 2f; 
            _agent.isStopped = false;
            _agent.ResetPath();
            // Random priority to prevent crowding
            _agent.avoidancePriority = Random.Range(20, 80); 
        }

        if (_rb != null) _rb.isKinematic = true;
        if (_anim != null) _anim.Rebind();

        SelectRandomHidePoint();

        StopAllCoroutines();
        StartCoroutine(DelayedStart());
    }

    void SelectRandomHidePoint()
    {
        if (SpawnManager.Instance != null && SpawnManager.Instance.hidepoints.Length > 0)
        {
            _selectedHidePoint = SpawnManager.Instance.hidepoints[Random.Range(0, SpawnManager.Instance.hidepoints.Length)];
        }
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();
        GoToHidePoint();
    }

    void Update()
    {
        if (_isDead || _agent == null) return;

        // If going to hideout (not yet hidden)
        if (!_finishedHiding && _selectedHidePoint != null)
        {
            // Current logic: if distance > 5m - walk speed 2, if less - run speed 5
            float dist = Vector3.Distance(transform.position, _selectedHidePoint.position);
            
            if (dist <= 10f && dist > _agent.stoppingDistance)
            {
                _agent.speed = 5f; // RUN
            }
            else
            {
                _agent.speed = 2f; // WALK
            }

            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                StartCoroutine(StayInHideout());
            }
        }

        // Pass speed to animator (2 for Walk, 5 for Run)
        _anim.SetFloat("Speed", _agent.velocity.magnitude);

        // If already finished hiding and reached finish - disable
        if (_finishedHiding && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (SpawnManager.Instance != null) SpawnManager.Instance.OnEnemyKilled();

            gameObject.SetActive(false); // Went to Finish
        }
    }

    void GoToHidePoint()
    {
        if (_selectedHidePoint != null)
        {
            _agent.SetDestination(_selectedHidePoint.position);
        }
    }

    IEnumerator StayInHideout()
    {
        _agent.isStopped = true;
        _anim.SetBool("Hiding", true);
        
        yield return new WaitForSeconds(Random.Range(2f, 5f));

        _anim.SetBool("Hiding", false);
        if (_agent != null && _agent.isOnNavMesh) _agent.isStopped = false;
        _finishedHiding = true; // Now go to finish
        
        // Send to finish point
        if (SpawnManager.Instance != null && SpawnManager.Instance.finishPoint != null)
        {
            _agent.speed = 2f; 
            _agent.SetDestination(SpawnManager.Instance.finishPoint.position);
        }
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;
        health -= amount;
        if (health <= 0) Die();
    }

    void Die()
    {
        _isDead = true;
        _agent.enabled = false;
        if (_rb != null) _rb.isKinematic = false;
        StopAllCoroutines();
        if (SpawnManager.Instance != null) SpawnManager.Instance.OnEnemyKilled();
        if (UIManager.Instance != null) UIManager.Instance.UpdateScore(50);
        AudioManager.Instance?.PlayEnemyDeath();
        _anim.SetTrigger("Death");
        Invoke(nameof(Deactivate), 2.5f);
    }

    void Deactivate() => gameObject.SetActive(false);
}