using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    //SPAWN GRID SETTING
    [SerializeField]
    private int sizeX = 4;
    [SerializeField]
    private int sizeY = 4;
    [SerializeField]
    private float spacing = 0.05f;
    [SerializeField]
    private Transform enemySpawnContainer;
    [SerializeField]
    private Transform[] spawnPointList = null;

    //ENEMY SPAWN SETTING
    [SerializeField]
    private float spawnFrecuency = 0.8f;
    private float spawnTimer = 0.0f;

    //ENEMY SETTING
    [SerializeField]
    private int maxEnemysCount = 8;
    [SerializeField]
    private Transform enemyContainer;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject[] enemyPoolingList = null;

    [SerializeField]
    private Transform playerTransform;

    public bool canSpawn = false;

    private void Start()
    {
        CreateSpawnPointsGrid();
        CreatePoolingEnemys();
    }

    private void CreateSpawnPointsGrid()
    {
        var gridSize = new Vector2(sizeX, sizeY);
        spawnPointList = new Transform[sizeX * sizeY];

        var count = 0;
        for (var x = 0; x < gridSize.x; x++)
            for (var y = 0; y < gridSize.y; y++)
            {
                var spawnCreated = new GameObject();
                spawnCreated.name = "Spawn" + count;
                spawnCreated.transform.SetParent(enemySpawnContainer, false);
                spawnCreated.transform.localPosition = new Vector3(x * spacing, y * spacing, 0.0f);

                spawnPointList[count] = spawnCreated.transform;

                count++;
            }
    }

    private void CreatePoolingEnemys()
    {
        enemyPoolingList = new GameObject[maxEnemysCount];

        for (var i = 0; i < maxEnemysCount; i++)
        {
            var enemyCreated = Instantiate(enemyPrefab) as GameObject;
            enemyCreated.name = "Enemy " + i;
            enemyCreated.transform.SetParent(enemyContainer, false);

            var enemyBehavior = enemyCreated.GetComponent<EnemyBehavior>();
            enemyBehavior.playerTransform = playerTransform;

            enemyPoolingList[i] = enemyCreated;
            enemyCreated.SetActive(false);
        }
    }

    private void Update()
    {
        SpawnEnemyTimerCheck();
    }

    private void SpawnEnemyTimerCheck()
    {
        if (canSpawn)
        {
            spawnTimer += Time.deltaTime * Random.Range(0.1f, spawnFrecuency);

            if (spawnTimer >= Random.Range(1.0f, 2.0f))
            {
                spawnTimer = 0.0f;
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        var randomSpawnPoint = Random.Range(0, spawnPointList.Length);

        for (var i = 0; i < maxEnemysCount; i++)
            if (enemyPoolingList[i].activeSelf == false)
            {
                enemyPoolingList[i].SetActive(true);
                enemyPoolingList[i].transform.localPosition = spawnPointList[randomSpawnPoint].localPosition;
                return;
            }
    }
}
