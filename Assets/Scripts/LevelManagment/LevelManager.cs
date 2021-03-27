using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
    public GameLevel[] _gameLevels;

    private void Awake()
    {
        _gameLevels = GetComponentsInChildren<GameLevel>();
        playerTransform = FindObjectOfType<Player>().transform;
        for (int i = 0; i < _gameLevels.Length; i++)
        {
            if (i == 0)
                continue;
            _gameLevels[i].gameObject.SetActive(false);
        }
    }

    public void LoadNextLevel(int currentLevelIndex, bool isNextLevel)
    {
        _gameLevels[currentLevelIndex].gameObject.SetActive(false);
        int indexToLoad = isNextLevel == true ? currentLevelIndex + 1 : currentLevelIndex - 1;
        _gameLevels[indexToLoad].gameObject.SetActive(true);
        playerTransform.position = _gameLevels[indexToLoad].PlayerStartPosition;

    }
}
