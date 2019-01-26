using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTest : ObjectBasic
{
    private Object bubblePrefab;

    // Start is called before the first frame update
    void Start()
    {
        bubblePrefab = Resources.Load("Prefabs/Bubble");
    }

    public override void Throw(bool rightdir)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * 1, 7), ForceMode2D.Impulse);

        gameObject.GetComponent<Rigidbody2D>().AddTorque(100);
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
}
