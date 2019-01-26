using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaController : MonoBehaviour
{

    public PlayerPhase ArenaPhase { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var newArenaPhase = CheckArenaPhase();
        
        if (newArenaPhase != ArenaPhase)
        {
            ArenaPhase = newArenaPhase;
            //TODO transition
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
