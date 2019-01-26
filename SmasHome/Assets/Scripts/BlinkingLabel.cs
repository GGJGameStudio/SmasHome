using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlinkingLabel : MonoBehaviour
{
    private Text pressStartLabel;
    private FadeType fadeType;

    private float blinkSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        pressStartLabel = this.GetComponent<Text>();
        StartCoroutine(this.TweenAlpha());
    }

    private IEnumerator TweenAlpha()
    {
        while(true)
        {
            if (fadeType == FadeType.In)
            {
                if (pressStartLabel.color.a >= 1)
                {
                    fadeType = FadeType.Out;
                }
                else
                {
                    var newColor = pressStartLabel.color;
                    newColor.a += blinkSpeed;

                    pressStartLabel.color = newColor;
                }
            }
            else
            {
                if (pressStartLabel.color.a <= 0)
                {
                    fadeType = FadeType.In;
                }
                else
                {
                    var newColor = pressStartLabel.color;
                    newColor.a -= blinkSpeed;

                    pressStartLabel.color = newColor;
                }
            }

            yield return null;
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        //Output that the Button is in the submit stage
        Debug.Log("Submitted!");
    }

    private enum FadeType
    {
        In,
        Out
    }
}
