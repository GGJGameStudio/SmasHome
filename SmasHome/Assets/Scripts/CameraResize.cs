﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraResize : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera mainCam;
    private Vector3 InitialPos = new Vector3(0,0,0);
    private float CamMinSize = 1;
    private float CamMaxSize = 2;
    [SerializeField] private float RapportMinMaxSize = 3;
 

    void Start()
    {
        mainCam = GetComponent<Camera>();
        CamMinSize = mainCam.orthographicSize;
        CamMaxSize = CamMinSize * RapportMinMaxSize;
        InitialPos = transform.position;
        


    }

    // Update is called once per frame
    void Update()
    {
        CamMaxSize = CamMinSize * RapportMinMaxSize;
        int nbChild = transform.childCount;
        Vector3 MinBox = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 MaxBox = new Vector3(0.0f, 0.0f, 0.0f); ;


        GameObject[] playerS = GameObject.FindGameObjectsWithTag("Player");
        int i = 0;
        foreach (GameObject player in playerS)
        {
            Transform trans = player.transform;
            if (i == 0)
            {
                MinBox = trans.position;
                MaxBox = trans.position;
            }else
            {
                if(trans.position.x < MinBox.x)
                {
                    MinBox.x = trans.position.x;
                }

                if (trans.position.y < MinBox.y)
                {
                    MinBox.y = trans.position.y;
                }

                if (trans.position.y > MaxBox.y)
                {
                    MaxBox.y = trans.position.y;
                }

                if (trans.position.x > MaxBox.x)
                {
                    MaxBox.x = trans.position.x;
                }
            }
            i++;

        }

        Vector3 Diff = MaxBox - MinBox;
        Vector3 Middle = (MaxBox + MinBox) / 2; 
        float size = mainCam.orthographicSize;
        mainCam.orthographicSize = System.Math.Max(Diff.x,Diff.y)/2;
        if(mainCam.orthographicSize < CamMinSize)
        {
            mainCam.orthographicSize = CamMinSize;
        }
        mainCam.transform.position = new Vector3(Middle.x, Middle.y, mainCam.transform.position.z);

        size = mainCam.orthographicSize;

        if ((Middle.x - size < InitialPos.x - CamMaxSize) ||
          (Middle.y - size < InitialPos.y - CamMaxSize)  ||
          (Middle.x + size > InitialPos.x + CamMaxSize)  ||
          (Middle.y + size > InitialPos.y + CamMaxSize))
        {
            mainCam.transform.position = InitialPos;
            mainCam.orthographicSize = CamMaxSize;
        }




    }
}
