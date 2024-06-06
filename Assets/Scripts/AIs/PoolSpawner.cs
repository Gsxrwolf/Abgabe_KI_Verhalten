using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolSpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;

    [SerializeField] public PoolSpawner referenceOfOtherSpawner;

    [SerializeField] int enemyStartAmount;
    [SerializeField] int enemyRefillAmount;
    [SerializeField] int maxEnemyAmount = 2;

    [SerializeField] Vector3 cachePosition;

    public List<GameObject> activeEnemyList = new List<GameObject>();
    private List<GameObject> cacheEnemyList = new List<GameObject>();

    private float timer;
    [SerializeField] public float spawnRate;

    public static event Action LevelFinished;

    void Start()
    {
        InstantiateNewEnemies(enemyStartAmount);
    }

    void Update()
    {
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
        if (cacheEnemyList.Count <= 0 )
        {
            InstantiateNewEnemies(enemyRefillAmount);
        }
        newEnemy = cacheEnemyList.First();
        Vector3 spawnPosition = GetNewSpawnPosition();
        if (spawnPosition == Vector3.zero)
        {
            return;
        }
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
            newEnemy = Instantiate(enemyPrefab, cachePosition, transform.rotation, transform);
            newEnemy.SetActive(false);

            WolfStateMachine wolfState;
            SheepStateMachine sheepState;

            if(newEnemy.TryGetComponent<WolfStateMachine>(out wolfState))
                WolfStateMachine.spawner = this;
            if (newEnemy.TryGetComponent<SheepStateMachine>(out sheepState))
                SheepStateMachine.spawner = this;

            cacheEnemyList.Add(newEnemy);
        }
    }
    public void DespawnEnemy(GameObject _enemy)
    {
        activeEnemyList.Remove(_enemy);

        _enemy.transform.position = cachePosition;
        _enemy.SetActive(false);
            cacheEnemyList.Add(_enemy);
    }

    private Vector3 GetNewSpawnPosition()
    {
        Vector3 spawnPosition = transform.position;
        System.Random rnd = new System.Random();


        return spawnPosition;
    }
}
