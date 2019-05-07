using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour 
{
    private Vector3 spawnPosition;
    private int numberOfEnemies = 7;
    private float spawnRadius = 5.0f;
    public EnemyAI[] enemyPrefabs;
    private int spawnRate = 20;
    private int spawnRatecounter = 0;

    GameManager mainGame;

    private GameObject[] spawnPoints;

    void FixedUpdate()
    {
        spawnRatecounter++;

        if (mainGame.CurrentGameState == GameState.On)
        {
            if (spawnRatecounter > spawnRate && mainGame.Enemies.Count < numberOfEnemies)
            {
                SpawnEnemy(spawnPoints[Random.Range(0, spawnPoints.Length)], enemyPrefabs[Random.Range(0,enemyPrefabs.Length)]);

                spawnRatecounter = 0;
            }
        }
    }

	// Use this for initialization
	void Start () 
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");
        mainGame = GameManager.MainGame;
        numberOfEnemies = mainGame.NumberOfEnemies;
	}

    void SpawnEnemy(GameObject spawnPoint, EnemyAI enemyPrefab)
    {
        spawnPosition = spawnPoint.transform.position + Random.insideUnitSphere * spawnRadius;
		mainGame.AddEnemy(Instantiate(enemyPrefab, spawnPosition, spawnPoint.transform.rotation));
    }
}
