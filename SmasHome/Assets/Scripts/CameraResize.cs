using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraResize : MonoBehaviour
{
    // Start is called before the first frame update

    Camera mainCam;

    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        int nbChild = transform.childCount;
        Vector3 MinBox = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 MaxBox = new Vector3(0.0f, 0.0f, 0.0f); ;


        for (int i = 0;i < nbChild;i++)
        {
            Transform trans = transform.GetChild(i);
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

        }

        Vector3 Diff = MaxBox - MinBox;
        Vector3 Middle = (MaxBox + MinBox) / 2; 
        float size = mainCam.orthographicSize;
        if (Diff.x > size)
        {
             size = Diff.x;
        }

        if (Diff.y > size)
        {
            size = Diff.y;
        }

        mainCam.orthographicSize = System.Math.Max(Diff.x,Diff.y)/2;
        mainCam.transform.position = new Vector3(Middle.x, Middle.y, mainCam.transform.position.z);
        

    }
}
