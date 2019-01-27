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
    [HideInInspector]
    public float Timer;

    public float Mass = 1;
    public float ThrowDamage = 1;
    public float StrikeDamage = 2;
    public float LifeTime = -1;
    public List<PlayerPhase> Available;

    private Vector3 startPosition;

    private GameObject arena;

    public Object BubblePrefab;
    public Object InfiniteObject;
    public bool ExplodeOnHit;
    public bool StickOnHit;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Owner = -1;
        Flying = false;
        Striking = false;
        Timer = 0f;
        
        startPosition = transform.position;

        arena = GameObject.FindGameObjectWithTag("Arena");

        if (gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
        {
            gameObject.GetComponent<Rigidbody2D>().mass = Mass;
        }
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

        if (Owner == -1 && gameObject.layer != LayerMask.NameToLayer("ObjectBack"))
        {
            Timer += Time.deltaTime;
            if (LifeTime != -1 && Timer > LifeTime)
            {
                Explode();
                Destroy(gameObject);
            }
        }


        PostUpdate();
    }

    public void Reset()
    {
        transform.position = startPosition;
        Timer = 0f;
        if (LifeTime != -1 && gameObject.layer != LayerMask.NameToLayer("ObjectBack"))
        {
            Destroy(gameObject);
        }

        transform.parent = null;
    }

    public void UpdateGrab(bool rightdir)
    {
        if (rightdir)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        } else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
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

        if (BubblePrefab != null)
        {
            var bubblePos = player.transform.position;

            bubblePos.x += 0.5f;
            bubblePos.y += 0.5f;

            var bubble = Instantiate(BubblePrefab, bubblePos, Quaternion.identity) as GameObject;

            if (bubble != null)
            {
                bubble.GetComponent<BasicBubbleBehaviour>().Pop();
            }
        }
        
    }

    public virtual void StrikeHit(GameObject player)
    {
        player.GetComponent<PlayerController>().Age += StrikeDamage;

        if (BubblePrefab != null)
        {
            var bubblePos = player.transform.position;

            bubblePos.x += 0.5f;
            bubblePos.y += 0.5f;

            var bubble = Instantiate(BubblePrefab, bubblePos, Quaternion.identity) as GameObject;

            if (bubble != null)
            {
                bubble.GetComponent<BasicBubbleBehaviour>().Pop();
            }
        }
    }

    public virtual void Explode()
    {
        //rien
    }

    public virtual void PostUpdate()
    {
        //rien
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("trigger");
        if (col.gameObject.tag == "Grab")
        {
            col.gameObject.GetComponent<Grab>().CanGrab.Add(gameObject);
        }

        if (col.gameObject.tag == "Player" && Striking && col.gameObject.GetComponent<PlayerController>().PlayerNumber != Owner)
        {
            Debug.Log("hit");
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

    void OnCollisionEnter2D(Collision2D col)
    {
        var obj = col.collider.gameObject;
        if (obj.tag == "Floor" || obj.tag == "Player" || obj.tag == "Object")
        {
            if (ExplodeOnHit && Flying)
            {
                Explode();
                Destroy(gameObject);
            }

            if (StickOnHit && Flying)
            {
                var contact = col.contacts[0];
                if (obj.tag == "Floor")
                {
                    gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                } else
                {
                    gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                }

                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Back";
                gameObject.GetComponent<ObjectBasic>().Timer = 0f;
                gameObject.layer = LayerMask.NameToLayer("ObjectBack");
                var delta = gameObject.transform.position - new Vector3(contact.point.x, contact.point.y, gameObject.transform.position.z);
                gameObject.transform.position = gameObject.transform.position - (delta * 0.5f);
                var angle = Mathf.Atan2(delta.y, delta.x) * 180 / Mathf.PI;
                gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 45 + 180));
                gameObject.transform.parent = obj.transform;
                gameObject.transform.localScale = new Vector3(1 / obj.transform.localScale.x, 1 / obj.transform.localScale.y, 1 / obj.transform.localScale.z);
            }
        }
    }
}
