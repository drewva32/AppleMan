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

    public void OnEnable()
    {
        _objectPooler = ObjectPooler.Instance;
        _enabledTime = Time.time;
    }

    public void OnDisable()
    {
        if(_objectPooler != null)
            _objectPooler.DespawnObjects();
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
