using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
    public static int currentDifficultyTokens { private set; get; }
    public static int currentDifficulty { private set; get; }
    [SerializeField] GameObject playerRef;
    [SerializeField] float spawnInterval = 10;
    float timer;
    GameObject[] spawnPoints;
    [Space(10)]
    [SerializeField] float minSpawnRadius = 14;
    [SerializeField] float maxSpawnRadius = 45;
    [SerializeField] bool drawGizmos;
    [Space(10)]
    [SerializeField] int maxEnemyCount = 20;
    int minSpawnRate = 3;
    [SerializeField] TilesetCard tilesetCard;
    List<EnemyCard> enemyCards = new List<EnemyCard>();
    List<EnemyHealth> aliveEnemies = new List<EnemyHealth>();

    private void Start()
    {
        if (playerRef == null)
            playerRef = GameManager.i.player;
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        enemyCards = tilesetCard.enemyCards;

        currentDifficulty = currentDifficultyTokens / 5 + 1;
        timer = spawnInterval;
    }

    public static EventHandler OnDifficultyIncrease;
    private void IncreaseDifficulty()
    {
        int oldDifficulty = currentDifficulty;

        SpawnEnemies();
        currentDifficultyTokens++;
        currentDifficulty = currentDifficultyTokens / 5 + 1;
        if (oldDifficulty != currentDifficulty)
            OnDifficultyIncrease?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        //timer
        if (timer > 0)
            timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = spawnInterval;
            IncreaseDifficulty();
        }
    }

    private void SpawnEnemies()
    {
        //checks for each dead enemy
        List<EnemyHealth> deadList = new List<EnemyHealth>();

        foreach (var item in aliveEnemies)
            if (item == null)
                deadList.Add(item);
        foreach (var item in deadList)
            aliveEnemies.Remove(item);
        int aliveEnemiesCount = aliveEnemies.Count();
        //if they hit the maximum enemy count then dont spawn any
        if (aliveEnemiesCount >= maxEnemyCount) return;

        //returns with a list of all possible spawnpoints in between min and max spawn radius
        List<Transform> spawnablePoints = new List<Transform>();
        foreach (GameObject point in spawnPoints)
        {
            if (!point.activeSelf) continue;
            float distance = Vector3.Distance(playerRef.transform.position, point.transform.position);
            if (distance > maxSpawnRadius || distance < minSpawnRadius) continue;

            spawnablePoints.Add(point.transform);
        }

        //spawnrate is influenced by the current difficulty of the game
        int spawnRate = Mathf.Clamp(currentDifficulty - 1 + minSpawnRate, minSpawnRate, maxEnemyCount - aliveEnemiesCount);
        //return with a list of spawnrate amount of points that are the furthest from the player
        var furthestSpawnpoints = spawnablePoints.OrderByDescending(item => Vector3.Distance(playerRef.transform.position, item.transform.position)).Take(spawnRate);

        foreach (Transform point in furthestSpawnpoints)
        {
            //generates a random enemy
            var randomCard = enemyCards[GetRandomEnemyCardIndex()];
            for (int i = 0; i < randomCard.count; i++)
            {
                GameObject enemy = Instantiate(randomCard.entity);
                Vector3 spawnPosition = point.position;
                if (randomCard.count > 1)
                {
                    //offsets the spawn of group enemies (might cause some out of bounds stuff)
                    Bounds spawnBounds = new Bounds();
                    spawnBounds.center = point.position;
                    spawnBounds.size = new Vector3(1, 0, 1);
                    spawnPosition = new Vector3(
                        Random.Range(spawnBounds.min.x, spawnBounds.max.x), 0f,
                        Random.Range(spawnBounds.min.z, spawnBounds.max.z));
                }
                enemy.transform.position = spawnPosition;
                enemy.GetComponent<EnemyNavMesh>().findTargetImmediately = true;
                aliveEnemies.Add(enemy.GetComponent<EnemyHealth>());
            }
        }
    }

    private int GetRandomEnemyCardIndex()
    {
        int cardCount = enemyCards.Count;
        int weightingListLenght = 0;
        List<int> weightingIndexList = new List<int>();

        for (int i = 0; i < cardCount; i++)
        {
            //check if the card is range of current difficulty
            //if 0 it can spawn at any difficulty
            int minDifficulty = enemyCards[i].minDifficulty;
            int maxDifficulty = enemyCards[i].maxDifficulty;
            if ((currentDifficulty < minDifficulty && minDifficulty != 0) ||
                (currentDifficulty > maxDifficulty && maxDifficulty != 0))
                continue;

            //adds weighting amount of items into the list that represents the cards index
            int elementWeighting = enemyCards[i].weighting;
            for (int e = 0; e < elementWeighting; e++)
            {
                weightingIndexList.Add(i);
                weightingListLenght++;
            }
        }
        int randomIndex = weightingIndexList[Random.Range(0, weightingIndexList.Count)];

        return randomIndex;
    }


    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerRef.transform.position, minSpawnRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(playerRef.transform.position, maxSpawnRadius);
    }
}
