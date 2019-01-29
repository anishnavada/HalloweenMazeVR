using UnityEngine;
using System.Collections;

public class OnDeath : StateMachineBehaviour

{
    // What happens when you die
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Unkillable", true);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<Animator>().SetTrigger("GameOver");
    }
}

