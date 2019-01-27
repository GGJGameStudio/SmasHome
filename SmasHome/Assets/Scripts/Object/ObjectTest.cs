using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTest : ObjectBasic
{
    private Object bubblePrefab;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        bubblePrefab = Resources.Load("Prefabs/Bubble");
    }

    public override void Throw(bool rightdir, float throwtimer, float throwForceMultiplier)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * 2 * throwtimer * throwForceMultiplier, 12 * throwtimer * throwForceMultiplier), ForceMode2D.Impulse);

        gameObject.GetComponent<Rigidbody2D>().AddTorque(100 * throwtimer * throwForceMultiplier);
    }

    public override void StrikeHit(GameObject player)
    {
        base.StrikeHit(player);
    }

    public override void ThrowHit(GameObject player)
    {
        base.ThrowHit(player);
    }
}
