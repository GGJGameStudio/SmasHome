using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaController : MonoBehaviour
{

    public PlayerPhase ArenaPhase { get; set; }
    public float TimeScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ArenaPhase = PlayerPhase.BABY;
    }

    // Update is called once per frame
    void Update()
    {
        var newArenaPhase = CheckArenaPhase();
        
        if (newArenaPhase != ArenaPhase)
        {
            ArenaPhase = newArenaPhase;
            Debug.Log("transition");
            //TODO transition
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

        foreach (GameObject player in players)
        {
            var phase = player.GetComponent<PlayerController>().CurrentPhase;
            if (phase > maxPhase)
            {
                maxPhase = phase;
            }
        }
        return maxPhase;
    }
}
