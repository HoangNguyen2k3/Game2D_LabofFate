using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : StateMachineBehaviour
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
        if (slashManager.isAttacking.Value)
        {
            slashManager.animator.Play("Slash1" + slashManager.GetDirectionStr());
        }
    }

}
