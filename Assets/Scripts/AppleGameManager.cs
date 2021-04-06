using System;
using UnityEngine;

public class AppleGameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerAndLevelsPrefab;
    [SerializeField] private int startingLives;
    [SerializeField] private PlayerData playerData;
    
    private static AppleGameManager _instance;
    public static AppleGameManager Instance => _instance;

    public event Action<int> OnLivesChanged;
    public event Action<int> OnCoinsChanged;
    public event Action<PlayerHealthController> OnPlayerCloned;

    public Transform CurrentPlayerTransform => _currentPlayerTransform;
    public Player CurrentPlayer => _currentPlayer;
    
    private int _coins;
    private int _kills;
    private int _lives;
    private Transform _currentPlayerTransform;
    private Vector3 _levelPosition;
    private Player _currentPlayer;
    private LevelsAndPlayer _currentGame;

    private void Awake()
    {
        if(_instance != null)
            Destroy(gameObject);
        else
            _instance = this;
        
        TryGetLevelPosition();
        _lives = startingLives;
    }

    private void OnDestroy()
    {
        _instance = null;
        if(playerData != null) 
            playerData.amountOfJumps = 1;
    }
   
    public void AddCoin()
    {
        _coins++;
        OnCoinsChanged?.Invoke(_coins);
        if (_coins > 99)
        {
            _coins = 0;
            OnCoinsChanged?.Invoke(_coins);
            ChangeLives(true);
        }
    }
    
    public void AddKill()
    {
        _kills++;
    }
    public void ChangeLives(bool isOneUP)
    {
        if (isOneUP)
            _lives++;
        else
            _lives--;
        if (_lives < 0)
        {
            _lives = 0;
            GameOver();
            return;
        }
        OnLivesChanged?.Invoke(_lives);
        
    }

    private void GameOver()
    {
        var childLevels = GetComponentInChildren<LevelsAndPlayer>();
        if (childLevels == null)
            return;
        var child = childLevels.gameObject;
        //reset the game.
        //show game over
        //destroy currernt game prefab
        //load in a new one that is paused?
        //transition from game over to main menu
        
        child.transform.parent = null;
        Destroy(child);
        Debug.Log("Game over");
        var newGame = Instantiate(playerAndLevelsPrefab, _levelPosition, Quaternion.identity);
        newGame.transform.position = _levelPosition;
        newGame.transform.parent = transform;
        
        ResetGameState();
        ResetPlayer();
    }

    private void ResetGameState()
    {
        _lives = startingLives;
        _coins = 0;
        _kills = 0;
        OnCoinsChanged?.Invoke(_coins);
        OnLivesChanged?.Invoke(_lives);

        _currentGame = GetComponentInChildren<LevelsAndPlayer>();
        _currentPlayer = _currentGame.Player;
        _currentPlayerTransform = _currentPlayer.transform;
        OnPlayerCloned?.Invoke(_currentPlayer.PlayerHealthController);
    }

    private void ResetPlayer()
    {
        if (playerAndLevelsPrefab == null)
        {
            _currentPlayer = FindObjectOfType<Player>();
            _currentPlayerTransform = _currentPlayer.transform;
        }
    }
    
    private void TryGetLevelPosition()
    {
        var levelAndPlayer = GetComponentInChildren<LevelsAndPlayer>();
        if (levelAndPlayer != null)
        {
            _levelPosition = levelAndPlayer.transform.position;
        }
    }
    
}
