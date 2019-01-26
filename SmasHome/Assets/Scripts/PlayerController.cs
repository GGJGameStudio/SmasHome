﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private bool onFloor;

    public int PlayerNumber;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

            if (this.onFloor)
            {
                animator.SetBool("Running", true);
            }
        }
        else if (this.onFloor)
        {
            animator.SetBool("Running", false);
        }

        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetButtonDown("Jump" + PlayerNumber) && onFloor)
        {
            rigidbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            onFloor = false;

            animator.SetBool("Jumping", true);
        }
        else
        {
            animator.SetBool("Jumping", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Floor" || collision2D.gameObject.tag == "Player")
        {
            onFloor = true;
            animator.SetBool("Jumping", false);
            animator.SetBool("Landing", true);
        }
    }
}
