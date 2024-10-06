using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    private AttackController _attackController;

    private NavMeshAgent _unitAgent;
    public float AttackingDistance = 1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackController = animator.transform.GetComponent<AttackController>();
        _unitAgent = animator.transform.GetComponent<NavMeshAgent>();

        _attackController.SetFollowMaterial();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_attackController.TargetToAttack == null)
        {
            animator.SetBool("isFollowing", false);
        }
        else
        {
            if (animator.transform.GetComponent<UnitMovement>().IsCommandedToMove) return;
            _unitAgent.SetDestination(_attackController.TargetToAttack.position);
            animator.transform.LookAt(_attackController.TargetToAttack);

            var distanceFromTarget = Vector3.Distance(_attackController.TargetToAttack.position, animator.transform.position);
            if (!(distanceFromTarget < AttackingDistance)) return;
            _unitAgent.SetDestination(animator.transform.position);
            animator.SetBool("isAttacking", true);
        }
    }
}
