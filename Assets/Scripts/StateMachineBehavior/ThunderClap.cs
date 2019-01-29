using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderClap : StateMachineBehaviour

{
    // What happens when you die
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("The Relic").transform.GetComponents<AudioSource>()[2].PlayDelayed(0.5f);
    }
}

