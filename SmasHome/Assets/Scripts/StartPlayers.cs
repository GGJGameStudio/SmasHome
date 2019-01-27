using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class StartPlayers : MonoBehaviour
{
    public GameObject player3;
    public GameObject player4;

    // Start is called before the first frame update
    void Start()
    {
        if (Global.NbPlayers == 2)
        {
            player3.SetActive(false);
            player4.SetActive(false);
        }
        else if (Global.NbPlayers == 3)
        {
            player4.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
