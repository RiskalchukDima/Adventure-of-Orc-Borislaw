using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConditionControl : MonoBehaviour
{
    public GameObject losePannel;
    public GameObject winPannel;

    public void Restart()
    {
        losePannel.SetActive(false);
        SceneManager.LoadScene("SampleScene");
    }
}
