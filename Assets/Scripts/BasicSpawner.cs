using System.Collections.Generic;
using UnityEngine;

public class BasicSpawner : MonoBehaviour
{
    private Transform[] spawner;
    public GameObject[] enemies;

    public float spawnFreq;
    private float spawnTime;
    private float timer;
    public int difficulty;
    public int difficultyRating = 1;
    public LayerMask layerMask;

    private void Start()
    {
        spawner = GetComponentsInChildren<Transform>();
        spawnTime = spawnFreq;
    }

    private void Update()
    {
        if (timer < spawnTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            difficulty++;
            difficultyRating = (difficulty / 5) + 1;
            spawnFreq = Mathf.Clamp(spawnFreq - (difficultyRating - 1) * 0.1f, 2f, spawnTime);
            timer = 0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < difficultyRating; i++)
        {
            GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)]);
            Vector3 spawnLocation = spawner[Random.Range(0, spawner.Length)].position;
            spawnLocation = new Vector3(
                spawnLocation.x + Random.Range(-1f, 1f),
                spawnLocation.y,
                spawnLocation.z + Random.Range(-1f, 1f));
            
            enemy.transform.position = spawnLocation;
        }
    }
}
