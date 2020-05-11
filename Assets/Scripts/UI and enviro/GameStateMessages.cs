using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateMessages : MonoBehaviour
{
    public static GameStateMessages instance;

    public GameObject LevelCompleteMsg, MissionFailMsg, wave1Msg, wave2Msg, wave3Msg, lastPoleDstrydMsg;

    bool hasInvoked = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }

    public void ShowWave1Msg()
    {
        wave1Msg.SetActive(true);
        Invoke("DisableWave1", 5);
    }

    void DisableWave1()
    {
        wave1Msg.SetActive(false);
    }

    public void ShowWave2Msg()
    {
        wave2Msg.SetActive(true);
        Invoke("DisableWave2", 5);
    }

    void DisableWave2()
    {
        wave2Msg.SetActive(false);
    }

    public void ShowLasDestroyedMsg()
    {
        lastPoleDstrydMsg.SetActive(true);
        Invoke("DisableLstDstryd", 5);
    }

    void DisableLstDstryd()
    {
        lastPoleDstrydMsg.SetActive(false);
    }

    public void ShowWave3Msg()
    {
        wave2Msg.SetActive(true);
        Invoke("DisableWave3", 5);
    }

    void DisableWave3()
    {
        wave2Msg.SetActive(false);
    }

    public void ShowLevelComplete()
    {
        if (!hasInvoked)
        {
            LevelCompleteMsg.SetActive(true);
            hasInvoked = true;
            Invoke("LoadMainMenu", 5);
        }
    }

   /* public void ShowLevelFailed()
    {
        if (!hasInvoked)
        {
            MissionFailMsg.SetActive(true);
            hasInvoked = true;
            Invoke("LoadMainMenu", 1);
        }
    }*/

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
