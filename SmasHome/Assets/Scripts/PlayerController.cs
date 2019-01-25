using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private bool onFloor;

    public int PlayerNumber;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        onFloor = true;
    }

    // Update is called once per frame
    void Update()
    {
        var speed = 3;
        var horizontal = Input.GetAxis("Horizontal" + PlayerNumber);
        if (horizontal != 0)
        {
            transform.localPosition = transform.localPosition + new Vector3(horizontal * speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetButtonDown("Jump" + PlayerNumber) && onFloor)
        {
            rigidbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            onFloor = false;
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
