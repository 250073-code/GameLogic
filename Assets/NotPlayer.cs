using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NotPlayer : MonoBehaviour
{
    public List<Transform> waypoints;
    private NavMeshAgent _navMesh;
    private int _currentPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        _navMesh = GetComponent<NavMeshAgent>();

        _navMesh.SetDestination(waypoints[_currentPoint].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_navMesh.remainingDistance < 0.5f)
        {
            _currentPoint++;
            _navMesh.SetDestination(waypoints[_currentPoint].position);
        }
    }
}
