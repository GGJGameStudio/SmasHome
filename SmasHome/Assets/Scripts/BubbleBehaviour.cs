﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    public AnimationCurve curve;

    public void Pop()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        var duration = 0f;

        while (duration < 1f)
        {
            duration += Time.deltaTime;

            var currentScale = curve.Evaluate(duration);

            var scale = transform.localScale;

            scale.x = currentScale;
            scale.y = currentScale;
            scale.z = currentScale;

            transform.localScale = scale;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
