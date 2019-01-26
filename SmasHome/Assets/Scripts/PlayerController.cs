using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private bool onFloor;
    private bool front;
    private GameObject grabbed;


    public int PlayerNumber;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        onFloor = true;
        front = true;
        grabbed = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var speed = 3;

        //deplacement
        var horizontal = Input.GetAxis("Horizontal" + PlayerNumber);
        if (horizontal != 0)
        {
            transform.localPosition = transform.localPosition + new Vector3(horizontal * speed * Time.deltaTime, 0, 0);
        }

        //Jump
        if (Input.GetButtonDown("Jump" + PlayerNumber) && onFloor)
        {
            rigidbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            onFloor = false;
        }

        //changement de plan
        var vertical = Input.GetAxis("Vertical" + PlayerNumber);
        if (vertical != 0)
        {
            if (vertical > 0.9)
            {
                front = false;
            }
            if (vertical < -0.9)
            {
                front = true;
            }
        }
        
        if (front)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerFront");
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerFront";
        } else
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerBack";
            gameObject.layer = LayerMask.NameToLayer("PlayerBack");
        }

        //grab
        if (Input.GetButtonDown("Grab" + PlayerNumber))
        {
            if (grabbed == null)
            {
                var grab = gameObject.transform.Find("Grab").GetComponent<Grab>();
                if (grab.CanGrab.Count > 0)
                {
                    grabbed = grab.CanGrab[0];
                    grabbed.transform.parent = grab.transform;
                    grabbed.transform.localPosition = Vector3.zero;
                    grabbed.GetComponent<BoxCollider2D>().enabled = false;
                    grabbed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                }
            } else
            {
                grabbed.transform.parent = null;
                grabbed.GetComponent<BoxCollider2D>().enabled = true;
                grabbed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                grabbed.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3, 1), ForceMode2D.Impulse);
                grabbed = null;
            }
        }

        if (grabbed != null){
            grabbed.transform.localPosition = Vector3.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Floor" || collision2D.gameObject.tag == "Player")
        {
            onFloor = true;
        }
    }
}
