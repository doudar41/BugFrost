using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startLevel : MonoBehaviour
{



    public void StartLevel()
    {
        SceneManager.LoadScene("BugFrostLevel01");
    }


    public void ExitGame()
    {
        Application.Quit();
    }


}
