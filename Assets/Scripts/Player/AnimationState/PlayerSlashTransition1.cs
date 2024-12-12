using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashTransition1 : StateMachineBehaviour
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
        if (Input.GetMouseButtonDown(0) || slashManager.canCombo.Value)
        {
            slashManager.animator.Play("Slash2" + slashManager.GetDirectionStr());
            slashManager.fireSlash.Active(slashManager.MousePositionToUnitVector());
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var slashManager = animator.GetComponent<SlashManagerCombo>();
        if(slashManager != null&&slashManager.IsOwner) {
            slashManager.canCombo.Value = false;
            slashManager.isAttacking.Value = false;
            slashManager.canAttack.Value = true;

        }

    }
}
