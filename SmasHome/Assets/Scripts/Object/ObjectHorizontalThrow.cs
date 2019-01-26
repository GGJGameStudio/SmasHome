using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHorizontalThrow : ObjectBasic
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void Throw(bool rightdir, float throwtimer, float throwForceMultiplier)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * 18 * throwtimer, 3 * throwtimer), ForceMode2D.Impulse);
    }
}
