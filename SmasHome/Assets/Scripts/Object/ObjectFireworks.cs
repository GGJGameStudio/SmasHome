using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFireworks : ObjectBasic
{
    public Object ExplosionEffect;
    public float ExplosionDamage;

    private bool rightdir;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void Throw(bool rightdir, float throwtimer, float throwForceMultiplier)
    {
        this.rightdir = rightdir;
        transform.localRotation = Quaternion.Euler(0, (rightdir ? 0 : 180), 0);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * 5, 0), ForceMode2D.Impulse);
    }

    public override void Explode()
    {
        base.Explode();

        var explosion = Instantiate(ExplosionEffect, transform.position, Quaternion.identity) as GameObject;
        var center = explosion.transform.position;
        var hits = Physics2D.OverlapCircleAll(center, 1.5f);
        foreach(Collider2D col in hits)
        {
            var obj = col.gameObject;
            if (obj.tag == "Object" && obj != gameObject)
            {
                if (obj.GetComponent<ObjectBasic>().Owner == -1)
                {
                    obj.GetComponent<Collider2D>().enabled = true;
                    obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    obj.layer = LayerMask.NameToLayer("Object");
                    obj.GetComponent<Rigidbody2D>().AddForce(5 * (col.gameObject.transform.position - center).normalized, ForceMode2D.Impulse);
                    obj.GetComponent<ObjectBasic>().Flying = true;
                    obj.GetComponent<ObjectBasic>().Owner = -1;
                }
            }

            if (obj.tag == "Player")
            {
                obj.GetComponent<PlayerController>().Age += ExplosionDamage;
            }
        }
        Destroy(explosion, 0.3f);
    }
    
}
