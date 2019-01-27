using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBubbleBehaviour : BasicBubbleBehaviour
{
    public override void Pop()
    {
        var animator = GetComponent<Animator>(); ;
        
        animator.enabled = true;

        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length + 0.2f);
    }
}
