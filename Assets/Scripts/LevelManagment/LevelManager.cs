using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
    public GameLevel[] _gameLevels;

    private void Awake()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        InitializeLevels();
    }

    private void InitializeLevels()
    {
        _gameLevels = GetComponentsInChildren<GameLevel>();
        
        for (int i = 0; i < _gameLevels.Length; i++)
        {
            _gameLevels[i].LevelIndex = i;
            if (i == 0)
                continue;
            _gameLevels[i].gameObject.SetActive(false);
        }
    }

    public void LoadNextLevel(int currentLevelIndex, bool isNextLevel)
    {
        _gameLevels[currentLevelIndex].gameObject.SetActive(false);
        int indexToLoad = isNextLevel == true ? currentLevelIndex + 1 : currentLevelIndex - 1;
        indexToLoad = EnsureValidIndex(indexToLoad);
        _gameLevels[indexToLoad].gameObject.SetActive(true);
        playerTransform.position = _gameLevels[indexToLoad].GetSpawnPoint(isNextLevel);

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
}
