using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Idle : StateMachineBehaviour
{
    // private NetworkAnimator networkAnimator;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    /*    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
            networkAnimator=animator.GetComponent<NetworkAnimator>();
            }*/

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var slashManager = animator.GetComponent<SlashManagerCombo>();
        if (slashManager != null && slashManager.canAttack.Value)
        {
            slashManager.animator.Play("Attack1");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    /*    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            SlashManagerCombo.instance.canAttack = false;
        }*/

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
