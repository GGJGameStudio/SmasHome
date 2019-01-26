using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
