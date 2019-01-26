using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{
    public int Owner;
    public bool Flying = false;
    public bool Striking = false;

    private float ThrowDamage = 1;
    private float StrikeDamage = 2;

    // Start is called before the first frame update
    void Start()
    {
        Owner = -1;
        Flying = false;
        Striking = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Flying && gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 1 && gameObject.GetComponent<Rigidbody2D>().angularVelocity < 1)
        {
            Flying = false;
            Owner = -1;
        }
    }

    public virtual void Throw(bool rightdir, float throwtimer)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * 20 * throwtimer, 2 * throwtimer), ForceMode2D.Impulse);
    }

    public virtual void Strike(bool rightdir)
    {
        gameObject.transform.localPosition = new Vector3(0, -0.5f, 0);
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
    }

    public virtual void ThrowHit(GameObject player)
    {
        player.GetComponent<PlayerController>().Age += ThrowDamage;
    }

    public virtual void StrikeHit(GameObject player)
    {
        player.GetComponent<PlayerController>().Age += StrikeDamage;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Grab")
        {
            col.gameObject.GetComponent<Grab>().CanGrab.Add(gameObject);
        }

        if (col.gameObject.tag == "Player" && Striking && col.gameObject.GetComponent<PlayerController>().PlayerNumber != Owner)
        {
            StrikeHit(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Grab")
        {
            col.gameObject.GetComponent<Grab>().CanGrab.Remove(gameObject);
        }
    }
}
