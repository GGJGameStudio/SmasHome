using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{



    [SerializeField] public AudioSource Audio;
    [SerializeField] private AudioClip AudioClipBackGround;
    [SerializeField] private AudioClip AudioClipEnd;
     private MainSceneBehaviour mainScene;
     private bool first = true;

   
    
    // Start is called before the first frame update
    void Start()
    {
        mainScene = GameObject.Find("Canvas").GetComponent<MainSceneBehaviour>();
        Audio = GetComponent<AudioSource>();
        Audio.clip = AudioClipBackGround;
        Audio.loop = true;
        Audio.Play();

    }

    // Update is called once per frame
    void Update()
    {


        if (mainScene.GetOutScene() && first)
        {
            Audio.clip = AudioClipEnd;
            Audio.Pause();
            Audio.Play(0);
            Audio.loop = false;
            first = false;

        }
    }
}
