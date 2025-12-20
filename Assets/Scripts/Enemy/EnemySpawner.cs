using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemySpawnList = new List<GameObject>();
    public int maxEnemies = 8;
    private int currentEnemies;
    public static EnemySpawner Instance;
    void Start()
    {
        Instance = this;

        for (int i = 0; i < maxEnemies; i++) 
        {
            spawnEnemy(0); 
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void spawnEnemy(int enemyIndex)
    {
        
       float randomX = Random.Range(369, 350);
       float randomZ = Random.Range(-377, -355);

        Vector3 spawnPosition = new Vector3(randomX, 14, randomZ);

        Instantiate(enemySpawnList[enemyIndex], spawnPosition, Quaternion.identity);
        currentEnemies++;
    }
}
