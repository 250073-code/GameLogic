using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("Points")]
    public List<Transform> waypoints;
    public List<Transform> hidePoints;

    [Header("Speed Settings")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 4.0f;

    private NavMeshAgent _navMesh;
    private CapsuleCollider _collider;
    private Animator _anim;
    private int _currentWaypointIndex = 0;
    private bool _isHiding = false;

    void Start()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _collider = GetComponent<CapsuleCollider>();
        _anim = GetComponent<Animator>();
        _navMesh.speed = walkSpeed;

        if (waypoints.Count > 0)
            _navMesh.SetDestination(waypoints[_currentWaypointIndex].position);
    }

    void Update()
    {
        if (_isHiding) return;

        _anim.SetFloat("Speed", _navMesh.velocity.magnitude);

        ManageSpeed();

        if (!_navMesh.pathPending && _navMesh.remainingDistance < 0.5f)
        {
            if (hidePoints.Contains(waypoints[_currentWaypointIndex]))
            {
                StartCoroutine(HideRoutine());
            }
            else
            {
                NextPoint();
            }
        }
    }

    void ManageSpeed()
    {
        Transform target = waypoints[_currentWaypointIndex];
        if (hidePoints.Contains(target) && Vector3.Distance(transform.position, target.position) < 4.0f)
        {
            _navMesh.speed = runSpeed;
        }
        else
        {
            _navMesh.speed = walkSpeed;
        }
    }

    IEnumerator HideRoutine()
    {
        _isHiding = true;
        _navMesh.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _navMesh.isStopped = true;

        _anim.SetFloat("Speed", 0);
        _anim.SetBool("Hiding", true);

        yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));

        _anim.SetBool("Hiding", false);
        _navMesh.isStopped = false;
        _navMesh.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        
        NextPoint();
        _isHiding = false;
    }

    void NextPoint()
    {
        if (waypoints.Count == 0) return;
        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Count;
        _navMesh.SetDestination(waypoints[_currentWaypointIndex].position);
    }

    public void DeathState()
    {
        _anim.SetTrigger("Death");
        _navMesh.isStopped = true;
        _navMesh.enabled = false; 
        _collider.enabled = false;
        this.enabled = false;
        StartCoroutine(ReturnToPoolRoutine());
        Debug.Log("Enemy Down! +50 points.");
    }

    IEnumerator ReturnToPoolRoutine()
    {
        yield return new WaitForSeconds(3f);
        _collider.enabled = true;
        _navMesh.enabled = true;
        this.enabled = true;
        gameObject.SetActive(false);
    }
}