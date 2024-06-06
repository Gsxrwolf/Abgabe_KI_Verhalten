using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PoolSpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;

    [SerializeField] Transform[] spawnPoints;

    [SerializeField] public PoolSpawner referenceOfOtherSpawner;


    [SerializeField] float delayedStartTime;
    [SerializeField] bool SpawndDirect;

    [SerializeField] int enemyStartAmount;
    [SerializeField] int enemyRefillAmount;
    [SerializeField] int maxEnemyAmount = 2;

    [SerializeField] public Vector3 cachePosition;

    public List<GameObject> activeEnemyList = new List<GameObject>();
    private List<GameObject> cacheEnemyList = new List<GameObject>();

    private float timer;
    [SerializeField] public float spawnRate;

    public static event Action LevelFinished;

    private bool wait;
    void Start()
    {
        wait = true;
        Invoke("LateStart", delayedStartTime);
    }

    private void LateStart()
    {
        InstantiateNewEnemies(enemyStartAmount);
        if(SpawndDirect)
        {
            for(int i = 0; i < enemyStartAmount; i++)
            {
                SpawnNewEnemy();
            }
        }
        wait = false;
    }

    void Update()
    {
        if (wait)
        {
            return;
        }
        if (activeEnemyList.Count < maxEnemyAmount)
        {
            timer += Time.deltaTime;
            if (timer > spawnRate)
            {
                timer = 0;
                SpawnNewEnemy();
            }
        }
    }

    public void SpawnNewEnemy()
    {
        GameObject newEnemy;
        if (cacheEnemyList.Count <= 0)
        {
            InstantiateNewEnemies(enemyRefillAmount);
        }
        newEnemy = cacheEnemyList.First();
        Vector3 spawnPosition = GetNewSpawnPosition();
        if (spawnPosition == Vector3.zero && spawnPosition == cachePosition)
        {
            return;
        }
        NavMeshHit hit;
        bool temp = NavMesh.SamplePosition(spawnPosition, out hit, 10000f, NavMesh.AllAreas);
        spawnPosition = hit.position;
        newEnemy.transform.position = spawnPosition;
        cacheEnemyList.Remove(newEnemy);
        newEnemy.SetActive(true);
        activeEnemyList.Add(newEnemy);
    }
    private void InstantiateNewEnemies(int amount)
    {
        for (int i = 0; i < enemyStartAmount; i++)
        {
            GameObject newEnemy;
            NavMeshHit hit;
            bool temp = NavMesh.SamplePosition(cachePosition, out hit, 10000f, NavMesh.AllAreas);
            cachePosition = hit.position;
            newEnemy = Instantiate(enemyPrefab, cachePosition, transform.rotation, transform);
            newEnemy.SetActive(false);
            

            WolfStateMachine wolfState;
            SheepStateMachine sheepState;

            if (newEnemy.TryGetComponent<WolfStateMachine>(out wolfState))
                WolfStateMachine.spawner = this;
            if (newEnemy.TryGetComponent<SheepStateMachine>(out sheepState))
                SheepStateMachine.spawner = this;

            cacheEnemyList.Add(newEnemy);
        }
    }
    public void DespawnEnemy(GameObject _enemy)
    {
        activeEnemyList.Remove(_enemy);

        _enemy.GetComponent<NavMeshAgent>().enabled = false;
        _enemy.transform.position = cachePosition;
        _enemy.SetActive(false);
        cacheEnemyList.Add(_enemy);
    }

    private int counter = -1;
    private Vector3 GetNewSpawnPosition()
    {
        if(SpawndDirect)
        {
            counter++;
            return spawnPoints[counter].position;
        }
        Vector3 spawnPosition;
        System.Random rnd = new System.Random();

        spawnPosition = spawnPoints[rnd.Next(spawnPoints.Length)].position;
        return spawnPosition;
    }
}
