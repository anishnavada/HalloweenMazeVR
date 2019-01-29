using UnityEngine;
using System.Collections;

public class CompletePunch : StateMachineBehaviour {
    // Used in animation Controller.
    // Removes a punch from the queue whenever a punch is started.

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetInteger("Punch Queue", animator.GetInteger("Punch Queue") - 1);
	}
	
	
}