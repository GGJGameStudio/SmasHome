using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Object pooPrefab;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private bool onFloor;
    private bool front;
    private bool rightdir;
    private GameObject grabbed;
    private float pooTimer;
    private float strikeTimer;
    private float throwTimer;
    private bool throwing;
    [SerializeField]
    private AudioSource damageAudio;
    [SerializeField]
    private AudioSource jumpAudio;

    public Object TombStonePrefab;

    public PlayerGender Gender;

    #region sprites

    public Sprite BabySprite;

    public Sprite GirlSprite;
    public Sprite TeenGirlSprite;
    public Sprite WomanSprite;
    public Sprite GranySprite;

    public Sprite BoySprite;
    public Sprite TeenBoySprite;
    public Sprite ManSprite;
    public Sprite PapySprite;

    public Sprite GhostSprite;

    #endregion

    #region animators

    public RuntimeAnimatorController BabyAnimator;
           
    public RuntimeAnimatorController GirlAnimator;
    public RuntimeAnimatorController TeenGirlAnimator;
    public RuntimeAnimatorController WomanAnimator;
    public RuntimeAnimatorController GranyAnimator;
           
    public RuntimeAnimatorController BoyAnimator;
    public RuntimeAnimatorController TeenBoyAnimator;
    public RuntimeAnimatorController ManAnimator;
    public RuntimeAnimatorController PapyAnimator;
           
    public RuntimeAnimatorController GhostAnimator;

    #endregion

    private RuntimeAnimatorController CurrentPhaseAnimator
    {
        get
        {
            if (Gender == PlayerGender.Female)
            {
                switch (CurrentPhase)
                {
                    case PlayerPhase.BABY:
                        return BabyAnimator;
                    case PlayerPhase.CHILD:
                        return GirlAnimator;
                    case PlayerPhase.TEEN:
                        return TeenGirlAnimator;
                    case PlayerPhase.ADULT:
                        return WomanAnimator;
                    case PlayerPhase.OLD:
                        return GranyAnimator;
                    case PlayerPhase.GHOST:
                        return GhostAnimator;
                }
            }
            else
            {
                switch (CurrentPhase)
                {
                    case PlayerPhase.BABY:
                        return BabyAnimator;
                    case PlayerPhase.CHILD:
                        return BoyAnimator;
                    case PlayerPhase.TEEN:
                        return TeenBoyAnimator;
                    case PlayerPhase.ADULT:
                        return ManAnimator;
                    case PlayerPhase.OLD:
                        return PapyAnimator;
                    case PlayerPhase.GHOST:
                        return GhostAnimator;
                }
            }

            return null;
        }
    }

    private Sprite CurrentPhaseSprite
    {
        get
        {
            if (Gender == PlayerGender.Female)
            {
                switch (CurrentPhase)
                {
                    case PlayerPhase.BABY:
                        return BabySprite;
                    case PlayerPhase.CHILD:
                        return GirlSprite;
                    case PlayerPhase.TEEN:
                        return TeenGirlSprite;
                    case PlayerPhase.ADULT:
                        return WomanSprite;
                    case PlayerPhase.OLD:
                        return GranySprite;
                    case PlayerPhase.GHOST:
                        return GhostSprite;
                }
            }
            else
            {
                switch (CurrentPhase)
                {
                    case PlayerPhase.BABY:
                        return BabySprite;
                    case PlayerPhase.CHILD:
                        return BoySprite;
                    case PlayerPhase.TEEN:
                        return TeenBoySprite;
                    case PlayerPhase.ADULT:
                        return ManSprite;
                    case PlayerPhase.OLD:
                        return PapySprite;
                    case PlayerPhase.GHOST:
                        return GhostSprite;
                }
            }

            return null;
        }
    }

    public PlayerPhase CurrentPhase;
    public int PlayerNumber;
    public float Age;

    public float Speed;
    public float JumpForce;
    public float ThrowForce;
    public float MaxCarryWeigth;

    // Start is called before the first frame update
    void Start()
    {
        pooPrefab = Resources.Load("Prefabs/Objects/Poo");

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        onFloor = true;
        front = false;
        grabbed = null;
        rightdir = true;
        strikeTimer = 0f;
        CurrentPhase = PlayerPhase.BABY;
        throwTimer = 0f;
        throwing = false;
        ThrowForce = 1f;

        spriteRenderer.material.SetFloat("_IsBackground", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerPhase();

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

        if (grabbed != null)
        {
            grabbed.GetComponent<ObjectBasic>().UpdateGrab(rightdir);
        }

        //Jump
        if (Input.GetButtonDown("Jump" + PlayerNumber) && (onFloor || CurrentPhase == PlayerPhase.GHOST) && CurrentPhase > PlayerPhase.BABY)
        {
            //Debug.Log("jump");
            rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            onFloor = false;
            animator.SetBool("Jumping", true);
            animator.SetBool("Falling", true);
            jumpAudio.Play();
        }
        else
        {
            animator.SetBool("Jumping", false);
        }

        //changement de plan
        var vertical = Input.GetAxis("Vertical" + PlayerNumber);
        if (vertical != 0 && CurrentPhase != PlayerPhase.BABY)
        {
            if (vertical > 0.9)
            {
                front = false;
                spriteRenderer.material.SetFloat("_IsBackground", 1.0f);
            }
            if (vertical < -0.9)
            {
                front = true;
                spriteRenderer.material.SetFloat("_IsBackground", 0.0f);
            }
        }

        if (CurrentPhase != PlayerPhase.GHOST)
        {
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
        } else
        {
            gameObject.layer = LayerMask.NameToLayer("Ghost");
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerFront";
        }

        //grab
        var grab = gameObject.transform.Find("Grab").GetComponent<Grab>();
        grab.transform.localPosition = new Vector3((rightdir ? 1 : -1) * Mathf.Abs(grab.transform.localPosition.x), grab.transform.localPosition.y, grab.transform.localPosition.z);
        
        if (Input.GetButtonUp("Grab" + PlayerNumber) && throwing)
        {
            if (grabbed != null)
            {
                //throw
                for(int i = 0; i < grabbed.transform.childCount; i++)
                {
                    var child = grabbed.transform.GetChild(i).gameObject;
                    if (child.GetComponent<Rigidbody2D>() != null)
                    {
                        child.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        child.GetComponent<Rigidbody2D>().mass = child.GetComponent<ObjectBasic>().Mass;
                        child.GetComponent<ObjectBasic>().Flying = true;
                        child.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
                        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), child.GetComponent<Collider2D>(), true);
                        StartCoroutine(EnableCollision(gameObject.GetComponent<Collider2D>(), child.GetComponent<Collider2D>()));
                    }
                }
                
                grabbed.transform.parent = null;
                grabbed.GetComponent<Collider2D>().enabled = true;
                grabbed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                grabbed.GetComponent<Rigidbody2D>().mass = grabbed.GetComponent<ObjectBasic>().Mass;
                grabbed.layer = LayerMask.NameToLayer("Object");
                grabbed.GetComponent<ObjectBasic>().Throw(rightdir, throwTimer, ThrowForce);
                grabbed.GetComponent<ObjectBasic>().Flying = true;
                grabbed.GetComponent<ObjectBasic>().Owner = -1;
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), grabbed.GetComponent<Collider2D>(), true);
                StartCoroutine(EnableCollision(gameObject.GetComponent<Collider2D>(), grabbed.GetComponent<Collider2D>()));
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
                    while (grab.CanGrab.Count > 0 && grab.CanGrab[0] == null)
                    {
                        grab.CanGrab.RemoveAt(0);
                    }

                    if (grab.CanGrab[0].GetComponent<ObjectBasic>().InfiniteObject == null)
                    {
                        //grab object
                        grabbed = grab.CanGrab[0];
                    } else
                    {
                        //grab infinite object
                        grabbed = Instantiate(grab.CanGrab[0].GetComponent<ObjectBasic>().InfiniteObject, transform.position, Quaternion.identity) as GameObject;
                    }
                    
                    if (grabbed.GetComponent<ObjectBasic>().Mass <= MaxCarryWeigth)
                    {
                        grabbed.transform.parent = grab.transform;
                        grabbed.transform.localPosition = Vector3.zero;
                        grabbed.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);
                        grabbed.GetComponent<Collider2D>().enabled = false;
                        grabbed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                        grabbed.GetComponent<ObjectBasic>().Owner = PlayerNumber;
                        grabbed.GetComponent<ObjectBasic>().Timer = 0f;
                        grabbed.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
                        grabbed.layer = LayerMask.NameToLayer("Object");
                    } else
                    {
                        if (grabbed != null)
                        {
                            Destroy(grabbed);
                        }
                        grabbed = null;
                        //todo feedback "trop lourd"
                    }
                    
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

        // Poo time!!!
        pooTimer -= Time.deltaTime;
        if (pooTimer < 0 && CurrentPhase == PlayerPhase.BABY && Input.GetButton("Strike" + PlayerNumber))
        {
            pooTimer = 2f;

            var pooPos = transform.position;
            pooPos.x += rightdir ? -.5f : .5f;

            var poo = Instantiate(pooPrefab, pooPos, Quaternion.identity) as GameObject;
            poo.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? -1 : 1) * 5, 5), ForceMode2D.Impulse);
            poo.GetComponent<ObjectBasic>().Flying = true;
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), poo.GetComponent<Collider2D>(), true);
            StartCoroutine(EnableCollision(gameObject.GetComponent<Collider2D>(), poo.GetComponent<Collider2D>()));
        }

        //strike
        strikeTimer -= Time.deltaTime;
        if (Input.GetButton("Strike" + PlayerNumber) && strikeTimer < 0)
        {
            if (grabbed != null)
            {
                grabbed.GetComponent<ObjectBasic>().Strike(rightdir);
                grabbed.GetComponent<Collider2D>().enabled = true;
                grabbed.GetComponent<Collider2D>().isTrigger = true;
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
            grabbed.GetComponent<Collider2D>().enabled = false;
            grabbed.GetComponent<Collider2D>().isTrigger = false;
            grabbed.GetComponent<ObjectBasic>().Striking = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Floor" || collision2D.gameObject.tag == "Player")
        {
            onFloor = true;
            animator.SetBool("Falling", false);

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
        PlayerPhase newphase;

        if (Age < 3)
        {
            newphase = PlayerPhase.BABY;
            CurrentPhase = newphase;
            Speed = 1;
            JumpForce = 0;
            ThrowForce = 0.5f;
            MaxCarryWeigth = 1f;
        }
        else if (Age < 10)
        {
            newphase = PlayerPhase.CHILD;
            Speed = 2;
            JumpForce = 6;
            ThrowForce = 1.5f;
            MaxCarryWeigth = 2f;
        }
        else if (Age < 18)
        {
            newphase = PlayerPhase.TEEN;
            Speed = 3;
            JumpForce = 8;
            ThrowForce = 2f;
            MaxCarryWeigth = 3f;
            transform.localScale = new Vector3(1, 1.4f, 1);
        }
        else if (Age < 60)
        {
            newphase = PlayerPhase.ADULT;
            Speed = 3;
            JumpForce = 8;
            ThrowForce = 2.5f;
            MaxCarryWeigth = 3f;
            transform.localScale = new Vector3(1.2f, 1.6f, 1);
        }
        else if (Age < 100)
        {
            newphase = PlayerPhase.OLD;
            Speed = 2;
            JumpForce = 6;
            ThrowForce = 1.8f;
            MaxCarryWeigth = 2f;
            transform.localScale = new Vector3(1.2f, 1.4f, 1);
        }
        else
        {
            newphase = PlayerPhase.GHOST;
            Speed = 2;
            JumpForce = 0.6f;
            ThrowForce = 0f;
            rigidbody.gravityScale = 0.1f;
            MaxCarryWeigth = 2f;
        }

        if (newphase != CurrentPhase)
        {
            if(newphase == PlayerPhase.GHOST)
            {
                var tomb = Instantiate(TombStonePrefab, transform.position, Quaternion.identity) as GameObject;
            }

            CurrentPhase = newphase;
            UpdatePlayerSprite();
            Reset();
        }
    }

    private void UpdatePlayerSprite()
    {
        animator.runtimeAnimatorController = CurrentPhaseAnimator;
        spriteRenderer.sprite = CurrentPhaseSprite;
    }

    private IEnumerator EnableCollision(Collider2D collider1, Collider2D collider2)
    {
        yield return new WaitForSeconds(1f);
        if (collider1 != null && collider2 != null)
        {
            Physics2D.IgnoreCollision(collider1, collider2, false);
        }
    }

    public void Reset()
    {
        //lâche l'objet
        if (grabbed != null)
        {
            grabbed.transform.parent = null;
            grabbed.GetComponent<Collider2D>().enabled = true;
            grabbed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            grabbed.layer = LayerMask.NameToLayer("Object");
            grabbed.GetComponent<ObjectBasic>().Throw(rightdir, throwTimer, 0);
            grabbed.GetComponent<ObjectBasic>().Flying = true;
            grabbed.GetComponent<ObjectBasic>().Owner = -1;
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), grabbed.GetComponent<Collider2D>(), true);
            StartCoroutine(EnableCollision(gameObject.GetComponent<Collider2D>(), grabbed.GetComponent<Collider2D>()));
            grabbed = null;
            throwing = false;
        }
    }
}
