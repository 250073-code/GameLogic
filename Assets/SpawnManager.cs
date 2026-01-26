using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Compression;
using System.Runtime.ExceptionServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int poolSize = 10;
    private List<GameObject> _pool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            _pool.Add(obj);
        }
    }

    public GameObject GetPooledObjects()
    {
        foreach (GameObject obj in _pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;   
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator EnemySpawnRoutine()
    {
        while (true)
        {
            GameObject enemy = GetPooledObjects();

            if (enemy != null)
            {
                enemy.transform.position = new Vector3(26.92f, 0.56f, 0.641f);
                enemy.SetActive(true);
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
