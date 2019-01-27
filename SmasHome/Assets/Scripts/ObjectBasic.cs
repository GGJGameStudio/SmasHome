using System.Collections;
using Assets;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{
    [HideInInspector]
    public int Owner;
    [HideInInspector]
    public bool Flying = false;
    [HideInInspector]
    public bool Striking = false;
    public float Timer;

    public float ThrowDamage = 1;
    public float StrikeDamage = 2;
    public float LifeTime = -1;
    public List<PlayerPhase> Available;

    private Vector3 startPosition;

    private GameObject arena;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Owner = -1;
        Flying = false;
        Striking = false;
        Timer = 0f;

        startPosition = transform.position;

        arena = GameObject.FindGameObjectWithTag("Arena");
    }

    // Update is called once per frame
    void Update()
    {
        if (Flying && gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 1 && gameObject.GetComponent<Rigidbody2D>().angularVelocity < 1)
        {
            Flying = false;
        }

        if (Available.Contains(arena.GetComponent<ArenaController>().ArenaPhase))
        {
            gameObject.SetActive(true);
        } else
        {
            gameObject.SetActive(false);
        }

        if (Owner == -1)
        {
            Timer += Time.deltaTime;
            if (LifeTime != -1 && Timer > LifeTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Reset()
    {
        transform.position = startPosition;
        Timer = 0f;
    }

    public virtual void Throw(bool rightdir, float throwtimer, float throwForceMultiplier)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * 20 * throwtimer * throwForceMultiplier, 2 * throwtimer * throwForceMultiplier), ForceMode2D.Impulse);
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
