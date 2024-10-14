using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    private NavMeshAgent _unitAgent;
    private AttackController _attackController;

    public float StopAttackingDistance = 1.2f;

    public float attackRate = 2f;
    private float attackTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _unitAgent = animator.GetComponent<NavMeshAgent>();
        _attackController = animator.GetComponent<AttackController>();

        _attackController.MuzzleEffect.gameObject.SetActive(true);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_attackController.TargetToAttack == null ||
            animator.transform.GetComponent<UnitMovement>().IsCommandedToMove)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        LookAtTarget();

        //move while attacking
        //_unitAgent.SetDestination(_attackController.TargetToAttack.position);

        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = 1f / attackRate;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }

        var distanceFromTarget =
            Vector3.Distance(_attackController.TargetToAttack.position, animator.transform.position);
        if (distanceFromTarget > StopAttackingDistance || _attackController.TargetToAttack == null)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void Attack()
    {
        var damageToInflict = _attackController.UnitDamage;
        _attackController.TargetToAttack.GetComponent<Unit>().TakeDamage(damageToInflict);
        SoundManager.Instance.PlayInfantryAttackSound();
    }

    private void LookAtTarget()
    {
        var direction = _attackController.TargetToAttack.position - _unitAgent.transform.position;
        _unitAgent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = _unitAgent.transform.eulerAngles.y;
        _unitAgent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackController.MuzzleEffect.gameObject.SetActive(false);
    }
}
