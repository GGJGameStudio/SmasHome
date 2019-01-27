using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneBehaviour : MonoBehaviour
{
    private GameObject pressStartLabel;

    private GameObject[] nbPlayersLabel;
    private int currentLabel;
    private bool lockHorizontal;
    private bool outScene = false;

    public bool GetOutScene()
    {
        return outScene;
    }

    public GameObject CurrentNbPlayerLabelSelected
    {
        get
        {
            return nbPlayersLabel[currentLabel];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pressStartLabel = transform.Find("PressStartLabel").gameObject;

        nbPlayersLabel = new GameObject[]
        {
            transform.Find("2PlayersLabel").gameObject,
            transform.Find("3PlayersLabel").gameObject,
            transform.Find("4PlayersLabel").gameObject
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!outScene)
        {
            if (!pressStartLabel.activeSelf && (Input.GetButtonUp("Start1") || Input.GetButtonUp("Start2") || Input.GetButtonUp("Start3") || Input.GetButtonUp("Start4")))
            {
                Global.NbPlayers = int.Parse(CurrentNbPlayerLabelSelected.name.Substring(0, 1));
                outScene = true;
                StartCoroutine(ChangeScene());
            }

            if (pressStartLabel.activeSelf && (Input.GetButtonUp("Start1") || Input.GetButtonUp("Start2") || Input.GetButtonUp("Start3") || Input.GetButtonUp("Start4")))
            {
                pressStartLabel.SetActive(false);
                CurrentNbPlayerLabelSelected.SetActive(true);

                var axis = Input.GetAxis("Horizontal2");

            }

            if (!pressStartLabel.activeSelf && !lockHorizontal && (Input.GetAxis("Horizontal1") > 0 || Input.GetAxis("Horizontal2") > 0 || Input.GetAxis("Horizontal3") > 0 || Input.GetAxis("Horizontal4") > 0))
            {
                CurrentNbPlayerLabelSelected.SetActive(false);
                currentLabel = Mathf.Min(nbPlayersLabel.Length - 1, currentLabel + 1);
                CurrentNbPlayerLabelSelected.SetActive(true);
                lockHorizontal = true;
            }

            if (!pressStartLabel.activeSelf && !lockHorizontal && (Input.GetAxis("Horizontal1") < 0 || Input.GetAxis("Horizontal2") < 0 || Input.GetAxis("Horizontal3") > 0 || Input.GetAxis("Horizontal4") > 0))
            {
                CurrentNbPlayerLabelSelected.SetActive(false);
                currentLabel = Mathf.Max(0, currentLabel - 1);
                CurrentNbPlayerLabelSelected.SetActive(true);

                lockHorizontal = true;
            }

            if (lockHorizontal && Input.GetAxis("Horizontal1") == 0 && Input.GetAxis("Horizontal2") == 0 || Input.GetAxis("Horizontal3") > 0 || Input.GetAxis("Horizontal4") > 0)
            {
                lockHorizontal = false;
            }
        }
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(1);
    }
}
