using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashAttack2 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var slashManager = animator.GetComponent<SlashManagerCombo>();
        if (slashManager != null && slashManager.IsOwner)
        {
            slashManager.isAttacking.Value = true;
        }
            
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var slashManager = animator.GetComponent<SlashManagerCombo>();
        if (slashManager != null && slashManager.IsOwner)
        {
            slashManager.isAttacking.Value = false;
            slashManager.StartAttackCooldown();
        }
    }
}
