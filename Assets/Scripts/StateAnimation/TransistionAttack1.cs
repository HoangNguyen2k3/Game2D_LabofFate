using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransistionAttack1 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var slashManager = animator.GetComponent<SlashManagerCombo>();
        if (slashManager.IsOwner&&slashManager != null)
        {
            slashManager.canAttack.Value = false;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var slashManager = animator.GetComponent<SlashManagerCombo>();
        if (slashManager != null && (slashManager.canAttack.Value || slashManager.canCombo.Value))
        {
            slashManager.animator.Play("Attack2");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var slashManager = animator.GetComponent<SlashManagerCombo>();
        if (slashManager.IsOwner && slashManager != null)
        {
            slashManager.canCombo.Value = false;
            slashManager.canAttack.Value = false;
        }
    }
}
