using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player player;
    
    public GameLevel[] _gameLevels;
    private List<Checkpoint> _checkpoints = new List<Checkpoint>();
    
    private int _currentRoomIndex;
    private Transform playerTransform;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerTransform = player.transform;
        InitializeLevels();
    }

    private void Start()
    {
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
            if (i == 0)
                continue;
            _gameLevels[i].gameObject.SetActive(false);
        }
    }

    private void CheckForCheckPoint(int index)
    {
        var checkpoint = _gameLevels[index].Checkpoint;
        if (checkpoint != null)
        {
            _checkpoints.Add(checkpoint);
            // checkpoint.OnCheckpointPassed += AssignCheckpointIndex;
        }
    }

    private void AssignCheckpointIndex(Checkpoint currentCheckpoint)
    {
        currentCheckpoint.LevelIndex = _currentRoomIndex;
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
        if (_checkpoints.Count > 0)
            return _checkpoints.Last(t => t.Passed).LevelIndex;
        else
            return 0;
    }
}
