using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTest : ObjectBasic
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Throw(bool rightdir)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * 1, 7), ForceMode2D.Impulse);

        gameObject.GetComponent<Rigidbody2D>().AddTorque(100);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.GetComponent<Grab>().CanGrab.Add(gameObject);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        col.gameObject.GetComponent<Grab>().CanGrab.Remove(gameObject);
    }
}
