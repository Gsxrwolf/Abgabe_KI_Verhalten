using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PoolSpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] public PoolSpawner referenceOfOtherSpawner;


    [SerializeField] private float delayedStartTime;
    [SerializeField] private bool SpawndDirect;

    private float timer;
    [SerializeField] private float spawnRate;

    [SerializeField] private int enemyStartAmount;
    [SerializeField] private int enemyRefillAmount;
    [SerializeField] private int maxEnemyAmount = 2;


    [SerializeField] private Vector3 cachePosition;

    public List<GameObject> activeEnemyList = new List<GameObject>();
    private List<GameObject> cacheEnemyList = new List<GameObject>();

    [SerializeField] private float characterHight;



    private bool wait;
    void Start()
    {
        wait = true;
        Invoke("LateStart", delayedStartTime);
    }

    private void LateStart()
    {
        InstantiateNewEnemies(enemyStartAmount);
        if (SpawndDirect)
        {
            for (int i = 0; i < enemyStartAmount; i++)
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
        if (activeEnemyList.Count < maxEnemyAmount)
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
            if (NavMesh.SamplePosition(spawnPosition, out hit, 10f, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;
                spawnPosition.y += characterHight / 2;
                newEnemy.transform.position = spawnPosition;


                cacheEnemyList.Remove(newEnemy);
                newEnemy.SetActive(true);
                activeEnemyList.Add(newEnemy);
            }
            else
            {
                SpawnNewEnemy();
            }
        }
    }
    public void SpawnNewEnemy(Vector3 _spawnPos)
    {
        if (activeEnemyList.Count < maxEnemyAmount)
        {
            GameObject newEnemy;
            if (cacheEnemyList.Count <= 0)
            {
                InstantiateNewEnemies(enemyRefillAmount);
            }
            newEnemy = cacheEnemyList.First();
            Vector3 spawnPosition = _spawnPos;
            if (spawnPosition == Vector3.zero && spawnPosition == cachePosition)
            {
                return;
            }


            NavMeshHit hit;
            if(NavMesh.SamplePosition(spawnPosition, out hit, 10f, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;
                spawnPosition.y += characterHight / 2;
                newEnemy.transform.position = spawnPosition;


                cacheEnemyList.Remove(newEnemy);
                newEnemy.SetActive(true);
                activeEnemyList.Add(newEnemy);
            }
            else
            {
                SpawnNewEnemy(_spawnPos);
            }
        }
    }
    private void InstantiateNewEnemies(int amount)
    {
        for (int i = 0; i < enemyStartAmount; i++)
        {
            GameObject newEnemy;
            NavMeshHit hit;
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

    
    private Vector3 GetNewSpawnPosition()
    {
        int counter = -1;
        if (SpawndDirect)
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
