using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashAttack2 : StateMachineBehaviour
{
    private SlashManagerCombo slashManager;

    private bool SetSlashManager(Animator animator)
    {
        slashManager = animator.GetComponent<SlashManagerCombo>();
        return slashManager != null;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!SetSlashManager(animator)) return;
        slashManager.isAttacking.Value = true;
            
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!SetSlashManager(animator)) return;
        slashManager.isAttacking.Value = false;
        slashManager.StartAttackCooldown();
    }
}
