using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    public static KillCounter instance;

    [SerializeField] private Text killCounter;

    private int numOfKills = 0;

    public void Start()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    public void EnemyKilled()
    {
        numOfKills++;
        if (numOfKills == 30)
        {
            PollHandler.instance.WaveFinished();
            GameStateMessages.instance.ShowWave1Msg();
        }
        if (numOfKills == 60)
        {
            PollHandler.instance.WaveFinished();
            GameStateMessages.instance.ShowWave2Msg();
        }
        if (numOfKills == 90)
        {
            PollHandler.instance.WaveFinished();
            GameStateMessages.instance.ShowWave3Msg();
        }
        killCounter.text = numOfKills.ToString();
    }
}
