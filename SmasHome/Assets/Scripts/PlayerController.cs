using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private bool onFloor;
    private bool front;
    private bool rightdir;
    private GameObject grabbed;
    private float strikeTimer;
    private float throwTimer;
    private bool throwing;
    [SerializeField]
    private AudioSource damageAudio;
    [SerializeField]
    private AudioSource jumpAudio;

    public PlayerPhase CurrentPhase;
    public int PlayerNumber;
    public float Age;
    public float Speed;
    public float JumpForce;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        onFloor = true;
        front = true;
        grabbed = null;
        rightdir = true;
        strikeTimer = 0f;
        Age = 8f;
        CurrentPhase = PlayerPhase.BABY;
        throwTimer = 0f;
        throwing = false;

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerPhase();
        UpdatePlayerSprite();

        //deplacement
        var horizontal = Input.GetAxis("Horizontal" + PlayerNumber);
        if (horizontal != 0)
        {
            transform.localPosition = transform.localPosition + new Vector3(horizontal * Speed * Time.deltaTime, 0, 0);

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
            rightdir = false;
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
            rightdir = true;
        }

        

        //Jump
        if (Input.GetButtonDown("Jump" + PlayerNumber) && onFloor && CurrentPhase > PlayerPhase.BABY)
        {
            //Debug.Log("jump");
            rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            onFloor = false;
            animator.SetBool("Jumping", true);
            jumpAudio.Play();
        }
        else
        {
            animator.SetBool("Jumping", false);
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
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerBack";
            gameObject.layer = LayerMask.NameToLayer("PlayerBack");
        }

        //grab
        var grab = gameObject.transform.Find("Grab").GetComponent<Grab>();
        grab.transform.localPosition = new Vector3((rightdir ? 1 : -1) * Mathf.Abs(grab.transform.localPosition.x), grab.transform.localPosition.y, grab.transform.localPosition.z);
        

        if (Input.GetButtonUp("Grab" + PlayerNumber) && throwing)
        {
            if (grabbed != null)
            {
                grabbed.transform.parent = null;
                grabbed.GetComponent<BoxCollider2D>().enabled = true;
                grabbed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                grabbed.layer = LayerMask.NameToLayer("Object");
                grabbed.GetComponent<ObjectBasic>().Throw(rightdir, throwTimer);
                grabbed.GetComponent<ObjectBasic>().Flying = true;
                Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), grabbed.GetComponent<Collider2D>(), true);
                StartCoroutine(EnableCollision(gameObject.GetComponent<BoxCollider2D>(), grabbed.GetComponent<Collider2D>()));
                grabbed = null;
                throwing = false;
            }
        }

        if (Input.GetButtonDown("Grab" + PlayerNumber) && strikeTimer < 0)
        {
            if (grabbed == null)
            {
                if (grab.CanGrab.Count > 0)
                {
                    grabbed = grab.CanGrab[0];
                    grabbed.transform.parent = grab.transform;
                    grabbed.transform.localPosition = Vector3.zero;
                    grabbed.GetComponent<BoxCollider2D>().enabled = false;
                    grabbed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    grabbed.GetComponent<ObjectBasic>().Owner = PlayerNumber;
                }
            }
            else
            {
                throwTimer = 0f;
                throwing = true;
            }
        }

        if (throwing)
        {
            throwTimer += Time.deltaTime;
            if (throwTimer > 0.5f)
            {
                throwTimer = 0.5f;
            }
        }

        if (grabbed != null && strikeTimer < 0)
        {
            grabbed.transform.localPosition = Vector3.zero;
            grabbed.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        //strike
        strikeTimer -= Time.deltaTime;
        if (Input.GetButton("Strike" + PlayerNumber) && strikeTimer < 0)
        {
            if (grabbed != null)
            {
                grabbed.GetComponent<ObjectBasic>().Strike(rightdir);
                grabbed.GetComponent<BoxCollider2D>().enabled = true;
                grabbed.GetComponent<BoxCollider2D>().isTrigger = true;
                grabbed.GetComponent<ObjectBasic>().Striking = true;
                strikeTimer = 0.5f;
            }
            else
            {
                // ??
            }
        }
        if (strikeTimer < 0 && grabbed != null)
        {
            grabbed.GetComponent<BoxCollider2D>().enabled = false;
            grabbed.GetComponent<BoxCollider2D>().isTrigger = false;
            grabbed.GetComponent<ObjectBasic>().Striking = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Floor" || collision2D.gameObject.tag == "Player")
        {
            onFloor = true;
            animator.SetBool("Jumping", false);
            animator.SetBool("Landing", true);

            if (collision2D.gameObject.tag == "Player")
            {
                damageAudio.Play();
            }
        }
        else if (collision2D.gameObject.tag == "Object")
        {
            var obj = collision2D.gameObject.GetComponent<ObjectBasic>();
            
            if (obj.Flying)
            {
                obj.GetComponent<ObjectBasic>().ThrowHit(gameObject);
            }
        }
    }

    private void UpdatePlayerPhase()
    {
        if (Age < 3)
        {
            CurrentPhase = PlayerPhase.BABY;
            Speed = 1;
            JumpForce = 0;
        }
        else if (Age < 10)
        {
            CurrentPhase = PlayerPhase.CHILD;
            Speed = 2;
            JumpForce = 4;
        }
        else if (Age < 18)
        {
            CurrentPhase = PlayerPhase.TEEN;
            Speed = 3;
            JumpForce = 5;
        }
        else if (Age < 60)
        {
            CurrentPhase = PlayerPhase.ADULT;
            Speed = 3;
            JumpForce = 4;
        }
        else if (Age < 100)
        {
            CurrentPhase = PlayerPhase.OLD;
            Speed = 2;
            JumpForce = 3;
        }
        else
        {
            CurrentPhase = PlayerPhase.GHOST;
            Speed = 2;
        }
    }

    private void UpdatePlayerSprite()
    {
        //TODO
        switch (CurrentPhase)
        {
            case PlayerPhase.BABY:
                //spriteRenderer.sprite =
                //animator = 
                break;
            case PlayerPhase.CHILD:
                break;
            case PlayerPhase.TEEN:
                break;
            case PlayerPhase.ADULT:
                break;
            case PlayerPhase.OLD:
                break;
            case PlayerPhase.GHOST:
                break;
        }
    }

    private IEnumerator EnableCollision(Collider2D collider1, Collider2D collider2)
    {
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(collider1, collider2, false);
    }
}
