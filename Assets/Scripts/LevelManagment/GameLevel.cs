using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private Transform startSpawnPoint;
    [SerializeField] private Transform endSpawnPoint;

    // [Space] [Header("Optional Respawn point for start of levels")] [SerializeField]
    private Checkpoint _checkpoint;
    
    public Checkpoint Checkpoint => _checkpoint;
    

    public int LevelIndex { get; set; }
    public Vector3 StartSpawnPoint => startSpawnPoint.position;
    public Vector3 EndSpawnPoint => endSpawnPoint.position;

    private LevelManager _levelManager;
    
    private void Awake()
    {
        _levelManager = GetComponentInParent<LevelManager>();
        _checkpoint = GetComponentInChildren<Checkpoint>();
    }


    //level manager uses this to place the character at the start or end point depending on if we are loading the next level or previous one.
    public Vector3 GetSpawnPoint(bool isNextlevel)
    {
        return isNextlevel == true ? StartSpawnPoint : EndSpawnPoint;
    }

    public void LevelTransition(bool isNextLevel)
    {
        _levelManager.LoadNextLevel(LevelIndex, isNextLevel);
        if(isNextLevel)
            _levelManager.SetCurrentRoomIndex(LevelIndex + 1);
        else
            _levelManager.SetCurrentRoomIndex(LevelIndex - 1);
    }
   
}