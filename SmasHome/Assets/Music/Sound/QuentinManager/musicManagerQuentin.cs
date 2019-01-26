using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicManager : MonoBehaviour
{
    public AudioSource source;
    [SerializeField] public List<AudioClip> clips;
    bool pause = false;

    public int nextClip = 0;

    public void Start()
    {
        source = GetComponent<AudioSource>();

        source.PlayOneShot(clips[nextClip]);

    }

    public void setNextClip(int index)
    {
        nextClip = index;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            source.Pause();
            pause = true;
        }


            if (!source.isPlaying && !pause)
        {
            nextClip++;
            if(nextClip >= clips.Count)
            {
                nextClip = 0;
            }
            source.PlayOneShot(clips[nextClip]);
        }
    }

}
