using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : StateMachineBehaviour
{
    private AttackController _attackController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attackController = animator.transform.GetComponent<AttackController>();

        _attackController.SetIdleMaterial();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_attackController.TargetToAttack != null)
        {
            animator.SetBool("isFollowing", true);
        }
    }
}
