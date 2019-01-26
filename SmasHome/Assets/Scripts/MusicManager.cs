using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{



    [SerializeField] public AudioSource Audio;
    [SerializeField] private AudioClip AudioClipBackGround;
    [SerializeField] private AudioClip AudioClipEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        Audio.clip = AudioClipBackGround;
        Audio.loop = true;
        Audio.Play();

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Audio.clip = AudioClipEnd;
            Audio.Pause();
            Audio.Play(0);
            Audio.loop = false;

        }
    }
}
