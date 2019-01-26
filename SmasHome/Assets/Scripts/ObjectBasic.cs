using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Throw(bool rightdir)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * 10, 2), ForceMode2D.Impulse);
    }

    public virtual void Strike(bool rightdir)
    {
        gameObject.transform.localPosition = new Vector3(0, -0.5f, 0);
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.GetComponent<Grab>().CanGrab.Add(gameObject);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        col.gameObject.GetComponent<Grab>().CanGrab.Remove(gameObject);
    }
}
