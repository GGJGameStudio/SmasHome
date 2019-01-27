using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFloBehaviour : MonoBehaviour
{
    public Object bubblePrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(this.Bim());
    }

    private IEnumerator Bim()
    {
        while (true)
        {
            (Instantiate(bubblePrefab, transform.position, Quaternion.identity) as GameObject)
                .GetComponent<BasicBubbleBehaviour>().Pop();

            yield return new WaitForSeconds(2f);
        }
    }
}
