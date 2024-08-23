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

    [SerializeField] private float maxXValueForSpawn = 630;
    [SerializeField] private float minXValueForSpawn = 450;
    [SerializeField] private float maxZValueForSpawn = 630;
    [SerializeField] private float minZValueForSpawn = 480;

    public List<GameObject> activeEnemyList = new List<GameObject>();
    private List<GameObject> cacheEnemyList = new List<GameObject>();

    [SerializeField] private float characterHight;
    [HideInInspector] public bool disableFur = false;


    public static event Action RoundIsOver;


    private bool startIsDone;
    void Start()
    {
        startIsDone = false;
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
        startIsDone = true;
    }

    void Update()
    {
        if (!startIsDone)
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
            Vector3 spawnPosition = GetNewSpawnPosition();
            if (spawnPosition == Vector3.zero && spawnPosition == cachePosition &&InvalidPos (spawnPosition))
            {
                return;
            }
            newEnemy = cacheEnemyList.First();
            


            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, 10f, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;
                newEnemy.transform.position = spawnPosition;

                NavMeshAgent agent = newEnemy.GetComponent<NavMeshAgent>();
                agent.enabled = true;
                agent.Warp(spawnPosition);


                cacheEnemyList.Remove(newEnemy);
                newEnemy.SetActive(true);
                activeEnemyList.Add(newEnemy);

                SheepStateMachine temp;
                if (newEnemy.TryGetComponent<SheepStateMachine>(out temp))
                    GameManager.Instance.AddPointsForSheepSpawn();


                WolfStateMachine temp2;
                if (newEnemy.TryGetComponent<WolfStateMachine>(out temp2))
                    GameManager.Instance.AddPointsForWolfSpawn();
            }
            else
            {
                SpawnNewEnemy();
            }
        }
    }
    private bool InvalidPos(Vector3 _destination)
    {
        if (_destination.x > maxXValueForSpawn)
            return true;
        if (_destination.x < minXValueForSpawn)
            return true;
        if (_destination.z > maxZValueForSpawn)
            return true;
        if (_destination.z < minZValueForSpawn)
            return true;

        return false;
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
            NavMesh.SamplePosition(spawnPosition, out hit, 10f, NavMesh.AllAreas);

            spawnPosition = hit.position;
            newEnemy.transform.position = spawnPosition;

            NavMeshAgent agent = newEnemy.GetComponent<NavMeshAgent>();
            agent.enabled = true;
            agent.Warp(spawnPosition);



            cacheEnemyList.Remove(newEnemy);
            newEnemy.SetActive(true);
            activeEnemyList.Add(newEnemy);
        }
    }
    private void InstantiateNewEnemies(int amount)
    {
        for (int i = 0; i < enemyStartAmount; i++)
        {
            GameObject newEnemy;
            newEnemy = Instantiate(enemyPrefab, cachePosition, transform.rotation, transform);
            newEnemy.SetActive(false);


            WolfStateMachine wolfStateMachine;
            SheepStateMachine sheepStateMachine;
            FurGenerator furGenerator;

            if (newEnemy.TryGetComponent<WolfStateMachine>(out wolfStateMachine))
                wolfStateMachine.spawner = this;
            if (newEnemy.TryGetComponent<SheepStateMachine>(out sheepStateMachine))
            {
                SheepStateMachine.spawner = this;
                GameObject bodyObj = sheepStateMachine.gameObject.transform.GetChild(2).gameObject;
                if (bodyObj.TryGetComponent<FurGenerator>(out furGenerator))
                    furGenerator.disableFur = disableFur;
            }



            cacheEnemyList.Add(newEnemy);
        }
    }
    public void DespawnEnemy(GameObject _enemy)
    {
        activeEnemyList.Remove(_enemy);

        _enemy.SetActive(false);
        _enemy.GetComponent<NavMeshAgent>().enabled = false;
        _enemy.transform.position = cachePosition;
        cacheEnemyList.Add(_enemy);

        if(activeEnemyList.Count <= 1)
        {
            SheepStateMachine sheepStateMachine;
            if (_enemy.TryGetComponent<SheepStateMachine>(out sheepStateMachine))
            {
                GameManager.Instance.lockScore = true;
                RoundIsOver?.Invoke();
                Time.timeScale = 0f;
            }
        }
    }


    private Vector3 GetNewSpawnPosition()
    {
        System.Random rnd = new System.Random();
        Vector3 spawnPosition;

        spawnPosition = spawnPoints[rnd.Next(spawnPoints.Length)].position;
        return spawnPosition;
    }
}
