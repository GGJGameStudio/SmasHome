using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSword : ObjectBasic
{


    public override void Throw(bool rightdir, float throwtimer, float throwForceMultiplier)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * 18 * throwtimer * throwForceMultiplier, 8 * throwtimer * throwForceMultiplier), ForceMode2D.Impulse);

        gameObject.GetComponent<Rigidbody2D>().AddTorque(150 + 50 * throwtimer * throwForceMultiplier);
    }
}
