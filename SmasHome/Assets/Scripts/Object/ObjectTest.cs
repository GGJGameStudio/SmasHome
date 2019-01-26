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

        gameObject.GetComponent<Rigidbody2D>().AddTorque(100 * throwtimer);
    }

    public override void StrikeHit(GameObject player)
    {
        base.StrikeHit(player);

        var bubblePos = player.transform.position;

        bubblePos.x += 0.5f;
        bubblePos.y += 0.5f;

        var bubble = Instantiate(bubblePrefab, bubblePos, Quaternion.identity) as GameObject;

        if (bubble != null)
        {
            bubble.GetComponent<BubbleBehaviour>().Pop();
        }
    }

    public override void ThrowHit(GameObject player)
    {
        base.ThrowHit(player);

        var bubblePos = player.transform.position;

        bubblePos.x += 0.5f;
        bubblePos.y += 0.5f;

        var bubble = Instantiate(bubblePrefab, bubblePos, Quaternion.identity) as GameObject;

        if (bubble != null)
        {
            bubble.GetComponent<BubbleBehaviour>().Pop();
        }
    }
}
