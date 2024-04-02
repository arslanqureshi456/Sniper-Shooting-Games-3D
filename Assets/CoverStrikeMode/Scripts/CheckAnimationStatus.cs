using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAnimationStatus : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("CanRoll", false);
    }
}
