using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBazooka : ObjectBasic
{
    public Object Rocket;

    private bool rightdir;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void Strike(bool rightdir)
    {
        var rocket = Instantiate(Rocket, transform.position, Quaternion.identity) as GameObject;
        rocket.GetComponent<Rigidbody2D>().gravityScale = 0;
        rocket.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * 5, 0), ForceMode2D.Impulse);
        rocket.GetComponent<ObjectBasic>().Flying = true;
        rocket.GetComponent<ObjectBasic>().Owner = -1;
        if (rightdir)
        {
            rocket.GetComponent<SpriteRenderer>().flipX = true;
        }
        
    }
}
