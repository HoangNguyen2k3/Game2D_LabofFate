using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashAttack1 : StateMachineBehaviour
{
    private SlashManagerCombo slashManager;

    private bool SetSlashManager(Animator animator)
    {
        slashManager = animator.GetComponent<SlashManagerCombo>();
        return slashManager != null;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!SetSlashManager(animator)) return;
        slashManager.isAttacking.Value = true;
        if (Input.GetMouseButtonDown(0))
        {
            slashManager.canCombo.Value = true;
        }
    }
}
