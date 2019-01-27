using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCaca : ObjectBasic
{
    [SerializeField] private List<AudioClip> clips;

    // Start is called before the first frame update


    protected override void Start()
    {
        base.Start();
        AudioSource audioPlayer = GetComponent<AudioSource>();
        int id = Random.Range((int)0, (int)clips.Count);
        audioPlayer.Stop();
        audioPlayer.loop = false;
        audioPlayer.clip = clips[id];
        audioPlayer.Play();

    }


    public override void Throw(bool rightdir, float throwtimer, float throwForceMultiplier)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((rightdir ? 1 : -1) * throwForceMultiplier * (8 - 6 * throwtimer), 30 * throwtimer * throwForceMultiplier), ForceMode2D.Impulse);

        gameObject.GetComponent<Rigidbody2D>().AddTorque(50 * throwtimer * throwForceMultiplier);
    }
}
