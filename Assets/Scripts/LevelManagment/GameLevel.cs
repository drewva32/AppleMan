using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private Transform playerStartPosition;
    [SerializeField] private int levelIndex;


    public Vector3 PlayerStartPosition => playerStartPosition.position;

    private LevelManager _levelManager;

    private void Awake()
    {
        _levelManager = GetComponentInParent<LevelManager>();
    }

    public void LevelTransition(bool isNextLevel)
    {
        _levelManager.LoadNextLevel(levelIndex, isNextLevel);
    }
   
}