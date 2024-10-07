using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Camera _cam;
    private NavMeshAgent _agent;
    public LayerMask ground;

    public bool IsCommandedToMove;

    Animator animator;

    private void Start()
    {
        _cam = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ground))
            {
                IsCommandedToMove = true;
                _agent.SetDestination(hit.point);
                animator.SetBool("isMoving", true);
            }
        }

        if (!_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance)
        {
            IsCommandedToMove = false;
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
    }
}
