using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalMelonSpawner : MonoBehaviour
{
    [SerializeField] private float spawnCoolDown = 0.5f;
    [SerializeField] private float startDelay;
    
    private float _lastSpawnTime;
    private ObjectPooler _objectPooler;
    private float _enabledTime;

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
        
    }

    public void OnEnable()
    {
        _enabledTime = Time.time;
        Debug.Log("enabled");
    }

    public void SpawnMelon()
    {
        _lastSpawnTime = Time.time;

        _objectPooler.SpawnFromPool(transform.position, transform.rotation);
    }

    private void Update()
    {
        if (Time.time < _lastSpawnTime + spawnCoolDown || Time.time < _enabledTime + startDelay)
            return;
        SpawnMelon();
    }
}
