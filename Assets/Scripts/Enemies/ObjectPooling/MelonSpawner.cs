using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MelonSpawner : MonoBehaviour
{
    [SerializeField] private float spawnCoolDown = 0.5f;
    
    [SerializeField] private Transform leftSpawnBounds;
    [SerializeField] private Transform rightSpawnBounds;
    [SerializeField] private float maxRandomAngle;

    private float _lastSpawnTime;
    private ObjectPooler _objectPooler;
    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(leftSpawnBounds.position.x, rightSpawnBounds.position.x);
        return new Vector3(randomX, transform.position.y,0);
    }

    private Quaternion GetRandomRotation()
    {
        float randomZRotation = Random.Range(-maxRandomAngle, maxRandomAngle);
        return Quaternion.Euler(0f,0f,180 + randomZRotation);
    }

    public void SpawnMelon()
    {
        _lastSpawnTime = Time.time;

        _objectPooler.SpawnFromPool(GetRandomPosition(), GetRandomRotation());
    }

    private void Update()
    {
        if (Time.time < _lastSpawnTime + spawnCoolDown)
            return;
        SpawnMelon();
    }
}
