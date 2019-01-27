using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var textComponent = transform.Find("Winner").gameObject.GetComponent<Text>();

        textComponent.text = string.Format(textComponent.text, Global.Winner);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Start1") || Input.GetButtonUp("Start2") || Input.GetButtonUp("Start3") || Input.GetButtonUp("Start4"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
