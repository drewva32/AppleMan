using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private Pool pool;
    private Queue<GameObject> objectPool = new Queue<GameObject>();

    #region Singleton

    private static ObjectPooler _instance;
    public static ObjectPooler Instance => _instance;

    private void Awake()
    {
        if(_instance != null)
            Destroy(gameObject);
        else
            _instance = this;
    }

    #endregion


    private void Start()
    {
        for (int i = 0; i < pool.poolSize; i++)
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = objectPool.Dequeue();
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        var pooledObject = objectToSpawn.GetComponent<IPooledObject>();
        pooledObject?.OnObjectSpawn();

        objectPool.Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
