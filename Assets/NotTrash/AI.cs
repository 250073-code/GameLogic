using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public enum AIState
    {
        Walking,
        Jumping,
        Attacking,
        Death
    }

    public Transform[] waypoints;
    private NavMeshAgent _navMeshAgent;
    private int _currentPoint = 0;
    private bool _isReverse = false;
    public AIState _currentState;
    public int health = 20;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.SetDestination(waypoints[_currentPoint].position);

        //_currentState = AIState.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _currentState = AIState.Jumping;
            _navMeshAgent.isStopped = true;
        }

        switch(_currentState)
        {
            case AIState.Walking:
                Debug.Log("Walking");
                CalculateMovement();
                break;
            case AIState.Jumping:
                Debug.Log("Jumping");
                break;
            case AIState.Attacking:
                Debug.Log("Attacking");
                break;
            case AIState.Death:
                Debug.Log("Death");
                break;
            
        }
    }

    void CalculateMovement()
    {
        if (_navMeshAgent.remainingDistance < 0.5f)
        {
            if (_isReverse == true)
            {
                Reverse();
            }
            else
            {
                Forward();
            }

            _navMeshAgent.isStopped = true;
            if (_navMeshAgent.isStopped == true)
            {
                StartCoroutine(Stop());
            }
        }

        _navMeshAgent.SetDestination(waypoints[_currentPoint].position);
    }

    void Forward()
    {
        if (_currentPoint == waypoints.Length - 1)
        {
            _isReverse = true;
            _currentPoint--;
        }
        else
        {
            _currentPoint++;
        }
    }

    void Reverse()
    {
        if (_currentPoint == 0)
        {
            _isReverse = false;
            _currentPoint++;
        }
        else
        {
            _currentPoint--;
        }
    }

    IEnumerator Stop()
    {
        _currentState = AIState.Attacking;
        yield return new WaitForSeconds(3);
        _navMeshAgent.isStopped = false;
    }
}
