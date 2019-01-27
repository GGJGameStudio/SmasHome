using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFloBehaviour : MonoBehaviour
{
    public Object bimPrefab;
    public Vector3 bimPosition;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(this.Bim());
    }

    private IEnumerator Bim()
    {
        while (true)
        {
            (Instantiate(bimPrefab, bimPosition, Quaternion.identity) as GameObject)
                .GetComponent<BubbleBehaviour>().Pop();

            yield return new WaitForSeconds(1.1f);
        }
    }
}
