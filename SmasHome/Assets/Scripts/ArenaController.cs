﻿using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaController : MonoBehaviour
{

    public PlayerPhase ArenaPhase { get; set; }
    public float TimeScale = 0.01f;

    public MusicManagerFightScene musicManager;
   

    // Start is called before the first frame update
    void Start()
    {
        ArenaPhase = PlayerPhase.BABY;
        GameObject musicManagerGameObj = GameObject.Find("Music");
        musicManager = musicManagerGameObj.GetComponent<MusicManagerFightScene>();
    }

    // Update is called once per frame
    void Update()
    {
        var newArenaPhase = CheckArenaPhase();
        
        if (newArenaPhase != ArenaPhase)
        {
            ArenaPhase = newArenaPhase;
            musicManager.setPhaseState(ArenaPhase);
            NewPhaseTransition();
           
        }

        //time
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().Age += Time.deltaTime * TimeScale;
        }
    }

    private PlayerPhase CheckArenaPhase()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        var maxPhase = PlayerPhase.BABY;
        var minPhase = PlayerPhase.GHOST;
        var nbGhost = 0;
        GameObject winner = null;

        foreach (GameObject player in players)
        {
            var phase = player.GetComponent<PlayerController>().CurrentPhase;
            if (phase > maxPhase)
            {
                maxPhase = phase;
            }

            if (phase < minPhase)
            {
                minPhase = phase;
                winner = player;
            }

            if (phase == PlayerPhase.GHOST || !player.activeSelf)
            {
                nbGhost++;
            }

            if (nbGhost >= Global.NbPlayers - 1)
            {
                Win(winner);
            }
        }

        if (maxPhase == PlayerPhase.GHOST)
        {
            maxPhase = PlayerPhase.OLD;
        }

        return maxPhase;
    }

    private void NewPhaseTransition()
    {
        var fondu = GameObject.FindGameObjectWithTag("Fondu");
        fondu.GetComponent<Fondu>().startFondu();

        var fonduspeed = fondu.GetComponent<Fondu>().Speed;
        StartCoroutine(DelayReset(fonduspeed));
    }

    private IEnumerator DelayReset(float speed)
    {
        yield return new WaitForSeconds(3.14f * 0.5f / speed);
        
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().Reset();
        }

        var objects = GameObject.FindGameObjectsWithTag("Object");
        foreach (GameObject obj in objects)
        {
            obj.GetComponent<ObjectBasic>().Reset();
        }
    }

    private void Win(GameObject winner)
    {
        Global.Winner = winner.GetComponent<PlayerController>().PlayerNumber;

        // TODO : confetis

        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(2);
    }
}
