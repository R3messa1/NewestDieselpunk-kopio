using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void ButtonPlayAgain()
    {
        SceneManager.LoadScene(1);
    }
    public void ButtonMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
}
