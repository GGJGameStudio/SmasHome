using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 1; i <= 4; i++)
        {
            var player = GameObject.Find("Player" + i);
            
            if (player != null)
            {
                canvas.transform.Find("Age" + i).GetComponent<Text>().text = player.GetComponent<PlayerController>().Age.ToString("F1");
            }
        }
    }
}
