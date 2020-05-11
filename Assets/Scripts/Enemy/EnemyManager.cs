using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    public int maxEnemies = 30;

    private int numOfEnemies;
    private int spawnPointIndex = 0;
    void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn()
    {
        if (numOfEnemies < maxEnemies)
        {
            spawnPointIndex++;
            if (spawnPointIndex >= spawnPoints.Length)
                spawnPointIndex = 0;
            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            numOfEnemies++;
        }
    }

    public void EnemyKilled()
    {
        numOfEnemies--;
    }

}
