using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour
{
    [Header("Optionally place this object on rooms where you want a checkpoint")]
    [Header("Place it at the StartSpawnPoint of the level, and mandatory for level 1")]
    // public event Action<Checkpoint> OnCheckpointPassed;
    private BoxCollider2D _collider;
    public bool Passed { get; private set; }
    public int LevelIndex { get;  set; }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        _collider.size = new Vector2(4, 4);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
            Passed = true;
        // OnCheckpointPassed?.Invoke(this);
    }

    public void ReceiveLevelIndex(int index)
    {
        LevelIndex = index;
    }
}
