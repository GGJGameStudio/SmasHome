using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCaca : ObjectBasic
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void Throw(bool rightdir, float throwtimer, float throwForceMultiplier)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * throwForceMultiplier * (8 - 6 * throwtimer), 30 * throwtimer * throwForceMultiplier), ForceMode2D.Impulse);

        gameObject.GetComponent<Rigidbody2D>().AddTorque(50 * throwtimer * throwForceMultiplier);
    }
}
