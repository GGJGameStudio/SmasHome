using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerFightScene : MonoBehaviour
{
    public AudioSource source;
    [SerializeField] public List<AudioClip> clips;
    public PlayerPhase phaseState = PlayerPhase.BABY;

    public void Start()
    {
        source = GetComponent<AudioSource>();
        source.loop = true;
        source.clip = clips[0];
        source.Play();

    }

    public void setPhaseState(PlayerPhase a_phaseState)
    {
        if ((a_phaseState != phaseState) && source.clip != clips[(int) a_phaseState])
        {
            phaseState = a_phaseState;
            source.Pause();
            source.clip = clips[(int)a_phaseState];
            source.Play();
        }
    }

    public void Update()
    {

    }

}
