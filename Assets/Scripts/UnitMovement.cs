using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Camera _cam;
    private NavMeshAgent _agent;
    public LayerMask Ground;

    public bool IsCommandedToMove;
    private DirectionIndicator _directionIndicator;

    private void Start()
    {
        _cam = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        _directionIndicator = GetComponent<DirectionIndicator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, Ground))
            {
                IsCommandedToMove = true;
                _agent.SetDestination(hit.point);

                _directionIndicator.DrawLine(hit);
            }
        }

        if (!_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance)
        {
            IsCommandedToMove = false;
        }
    }
}
