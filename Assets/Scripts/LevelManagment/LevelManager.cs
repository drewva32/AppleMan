using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [Header("Can activate this when testing a specific part of a level")]
    [SerializeField] private bool devMode;
    public GameLevel[] _gameLevels;
    private List<Checkpoint> _checkpoints = new List<Checkpoint>();
    
    private int _currentRoomIndex;
    private Transform playerTransform;
    private void Awake()
    {
        if(player == null)
            player = FindObjectOfType<Player>();
        playerTransform = player.transform;
    }

    private void Start()
    {
        InitializeLevels();
        player.PlayerHealthController.OnDie += LoadAtLastCheckPoint;
    }

    private void OnDisable()
    {
        player.PlayerHealthController.OnDie -= LoadAtLastCheckPoint;
    }

    private void InitializeLevels()
    {
        _gameLevels = GetComponentsInChildren<GameLevel>();
        
        for (int i = 0; i < _gameLevels.Length; i++)
        {
            CheckForCheckPoint(i);
            _gameLevels[i].LevelIndex = i;
            // if (i == 0)
            //     continue;
            _gameLevels[i].gameObject.SetActive(false);
        }
        LoadLevel(0);
    }

    private void CheckForCheckPoint(int index)
    {
        var checkpoint = _gameLevels[index].Checkpoint;
        if (checkpoint != null)
        {
            Debug.Log("VAR");
            _checkpoints.Add(checkpoint);
            checkpoint.ReceiveLevelIndex(index);
            //could give checkpoint its index here instead of having the game level do that in start
        }
    }

    public void LoadNextLevel(int transitioningFromIndex, bool isNextLevel)
    {
        _gameLevels[transitioningFromIndex].gameObject.SetActive(false);
        int indexToLoad = isNextLevel == true ? transitioningFromIndex + 1 : transitioningFromIndex - 1;
        indexToLoad = EnsureValidIndex(indexToLoad);
        _gameLevels[indexToLoad].gameObject.SetActive(true);
        playerTransform.position = _gameLevels[indexToLoad].GetSpawnPoint(isNextLevel);
    }

    public void LoadAtLastCheckPoint()
    {
        int indexToLoad = GetLastCheckpointLevelIndex();
        _gameLevels[_currentRoomIndex].gameObject.SetActive(false);
        _gameLevels[indexToLoad].gameObject.SetActive(true);
        playerTransform.position = _gameLevels[indexToLoad].GetSpawnPoint(true);
        SetCurrentRoomIndex(indexToLoad);
    }

    public void LoadLevel(int levelIndex)
    {
        _gameLevels[levelIndex].gameObject.SetActive(true);
        SetCurrentRoomIndex(levelIndex);
        if(!devMode)
            player.transform.position = _gameLevels[levelIndex].StartSpawnPoint;
    }

    //this function was 
    private int EnsureValidIndex(int indexToLoad)
    {
        //connects first level to last level
        if (indexToLoad < 0)
            indexToLoad = _gameLevels.Length - 1;
        //connects end level to first level
        else
            indexToLoad %= _gameLevels.Length;
        return indexToLoad;
    }

    public void SetCurrentRoomIndex(int index)
    {
        _currentRoomIndex = index;
    }

    public int GetLastCheckpointLevelIndex()
    {
        Debug.Log("checkpoint index" + _checkpoints.Count);
        if (_checkpoints.Count > 0)
            return _checkpoints.Last(t => t.Passed).LevelIndex;
        return 0;
    }
}
