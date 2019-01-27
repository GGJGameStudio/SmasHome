using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fondu : MonoBehaviour
{
    public SpriteRenderer sprite;
    public float Speed = 1.5f;
    private float startTime = 0.0f;
    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float delta = (Time.realtimeSinceStartup - startTime) * Speed;
        if (started && delta < 3.14f)
        {
            float alpha = 0.0f;
            alpha = Mathf.Sin(delta);
            sprite.material.SetFloat("_Alpha", alpha);
        }
        else if (started)
        {
            sprite.material.SetFloat("_Alpha", 0.0f);
            started = false;
        }
    }

    public void startFondu()
    {
        startTime = Time.realtimeSinceStartup;
        started = true;
    }
}
