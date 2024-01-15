using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{

    [Header("Reference")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves= 5f;
    [SerializeField] private float difficultieConst = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyKill = new UnityEvent() ;

    private int currentWave = 1;
    private float lastSpawnTime = 0f;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool spawningFlag = false;

    private void Awake()
    {
        onEnemyKill.AddListener(EnemyKilled);
    }

    private void EnemyKilled()
    {
        enemiesAlive--;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemiesLeftToSpawn = baseEnemies;
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        spawningFlag = true;
        enemiesLeftToSpawn = EnemiesInWave();
    }

    private int EnemiesInWave() {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultieConst));
    }
    // Update is called once per frame

    private void SpawnEnemy()
    {
        GameObject enemyToSpawn = enemyPrefabs[0];
        Instantiate(enemyToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private void EndWave()
    {
        spawningFlag = false;
        lastSpawnTime = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (!spawningFlag)
            return;
        lastSpawnTime += Time.deltaTime;
        if (lastSpawnTime >= 1f / enemiesPerSecond && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            lastSpawnTime = 0f;
        }
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }
}
