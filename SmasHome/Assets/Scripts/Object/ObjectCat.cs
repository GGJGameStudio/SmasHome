using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCat : ObjectBasic
{
    private Animator animator;
    private SpriteRenderer renderer;

    private float movetimer;
    private bool directionRight;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        directionRight = true;
        movetimer = 0;

        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public override void PostUpdate(float deltaTime)
    {
        animator.SetBool("Angry", Owner != -1 || Flying);
        renderer.flipX = !this.directionRight;

        if (directionRight)
        {
            movetimer += deltaTime;
        } else
        {
            movetimer -= deltaTime;
        }

        if (movetimer > 4)
        {
            directionRight = false;
        } else if (movetimer < 0)
        {
            directionRight = true;
        }

        gameObject.transform.position = new Vector3(gameObject.transform.position.x + (directionRight?1:-1) * deltaTime * 0.5f, gameObject.transform.position.y, gameObject.transform.position.z);

    }
}
