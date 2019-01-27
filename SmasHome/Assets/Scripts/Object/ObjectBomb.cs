using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBomb : ObjectBasic
{
    public Object ExplosionEffect;
    public float ExplosionDamage;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void Explode()
    {
        base.Explode();

        var explosion = Instantiate(ExplosionEffect, transform.position, Quaternion.identity) as GameObject;
        var center = explosion.transform.position;
        var hits = Physics2D.OverlapCircleAll(center, 1);
        foreach(Collider2D col in hits)
        {
            var obj = col.gameObject;
            if (obj.tag == "Object" && obj != gameObject)
            {
                obj.GetComponent<Collider2D>().enabled = true;
                obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                obj.layer = LayerMask.NameToLayer("Object");
                obj.GetComponent<Rigidbody2D>().AddForce(5 * (col.gameObject.transform.position - center).normalized, ForceMode2D.Impulse);
                obj.GetComponent<ObjectBasic>().Flying = true;
                obj.GetComponent<ObjectBasic>().Owner = -1;
            }

            if (obj.tag == "Player")
            {
                Debug.Log("ouch");
                obj.GetComponent<PlayerController>().Age += ExplosionDamage;
            }
        }
        Destroy(explosion, 0.3f);
    }
    
}
