using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Settings")]
    public GameObject enemyPrefab;
    public Transform startPoint;
    public Transform finishPoint;
    public Transform[] hidepoints;
    
    [Header("Wave Settings")]
    public float spawnInterval = 3f;
    public int totalEnemiesToSpawn = 25; // Total enemies to spawn
    
    // Tracking variables
    private int _spawnedCount = 0;
    private bool _allSpawned = false;
    public int enemiesAlive = 0;
    private float gameTime = 0f;
    private bool gameActive = true;

    [Header("Pooling")]
    public int poolSize = 25;
    private List<GameObject> _enemyPool;

    void Awake()
    {
        Instance = this;
        InitPool();
    }

    void Start()
    {
        // Update UI on start
        if (UIManager.Instance != null) UIManager.Instance.UpdateEnemyCount(enemiesAlive);
        
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (!gameActive) return;

        // Count time and send to UI
        gameTime += Time.deltaTime;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimeDisplay(gameTime);
        }
    }

    void InitPool()
    {
        _enemyPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, Vector3.zero, Quaternion.Euler(0, -90, 0));
            obj.SetActive(false);
            _enemyPool.Add(obj);
        }
    }

    public void SpawnEnemy()
    {
        if (_spawnedCount >= totalEnemiesToSpawn) return;
        
        GameObject enemy = GetPooledObject();
        if (enemy != null)
        {
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                enemy.transform.position = startPoint.position;
                agent.enabled = true;
                agent.Warp(startPoint.position);
            }
            enemy.SetActive(true);

            _spawnedCount++;
            enemiesAlive++;
            if (UIManager.Instance != null) UIManager.Instance.UpdateEnemyCount(enemiesAlive);
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
        if (enemiesAlive < 0) enemiesAlive = 0;
        
        if (UIManager.Instance != null) UIManager.Instance.UpdateEnemyCount(enemiesAlive);

        if (_allSpawned && enemiesAlive <= 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameActive = false; // Stop timer    
        UIManager.Instance.ShowWin();
    }

    IEnumerator SpawnRoutine()
    {
        while (_spawnedCount < totalEnemiesToSpawn)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }

        _allSpawned = true;
    }

    GameObject GetPooledObject()
    {
        foreach (GameObject obj in _enemyPool)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }
}