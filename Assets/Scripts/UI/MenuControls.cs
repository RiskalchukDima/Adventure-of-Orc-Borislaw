using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    public void StartPressed()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void OptionsPressed()
    {
    }
    public void ExitPressed()
    {
        Application.Quit();
    }
}
