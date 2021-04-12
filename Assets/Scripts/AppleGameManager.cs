using System;
using TMPro;
using UnityEngine;

public class AppleGameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerAndLevelsPrefab;
    [SerializeField] private int startingLives;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private TextMeshProUGUI transitionText;

    [Header("UI")] [SerializeField] private GameObject MainMenuScreen;
    [SerializeField] private GameObject gameOverScreen;
    
    
    private static AppleGameManager _instance;
    public static AppleGameManager Instance => _instance;

    public event Action<int> OnLivesChanged;
    public event Action<int> OnCoinsChanged;
    public event Action<PlayerHealthController> OnPlayerCloned;

    public Transform CurrentPlayerTransform => _currentPlayerTransform;

    public TextMeshProUGUI TransitionText => transitionText;

    public Player CurrentPlayer => _currentPlayer;
    public LevelsAndPlayer LevelsAndPlayer => _currentGame;
    public int lifeID { get; set; }

    private float _gameTime;
    private int _coins;
    private int _kills;
    private int _lives;
    private Transform _currentPlayerTransform;
    private Vector3 _levelPosition;
    private Player _currentPlayer;
    private LevelsAndPlayer _currentGame;
    private LevelManager _currentLevelManager;

    private void Awake()
    {
        if(_instance != null)
            Destroy(gameObject);
        else
            _instance = this;

        _currentGame = GetComponentInChildren<LevelsAndPlayer>();
        _currentPlayer = FindObjectOfType<Player>();
        _currentPlayerTransform = _currentPlayer.transform;
        TryGetLevelPosition();
        _lives = startingLives;
    }


    private void Start()
    {
        if (_currentGame != null)
        {
            _currentPlayer = _currentGame.Player;
            _currentLevelManager = _currentGame.LevelManager;
        }
    }

    private void Update()
    {
        _gameTime += Time.deltaTime;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        
        _currentLevelManager.LoadLevel(0);
    }

    public void MainMenu()
    {
        //pause time
        //fade in music that plays regardless of timescale
        AudioManager.Instance.MusicAudioController.FadeInThemeMusic();
        MainMenuScreen.SetActive(true);
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

    public void GameOver()
    {
        var childLevels = GetComponentInChildren<LevelsAndPlayer>();
        if (childLevels == null)
            return;
        OpenGameOverScreen();
        var child = childLevels.gameObject;
        //reset the game.
        //show game over
        //destroy currernt game prefab
        //load in a new one that is paused?
        //transition from game over to main menu
        
        child.transform.parent = null;
        Destroy(child);
        playerData.amountOfJumps = 1;
        Debug.Log("Game over");
        var newGame = Instantiate(playerAndLevelsPrefab, _levelPosition, Quaternion.identity);
        newGame.transform.position = _levelPosition;
        newGame.transform.parent = transform;
        

        ResetGameState();
        ResetPlayer();
        
    }

    private void OpenGameOverScreen()
    {
        if (gameOverScreen == null)
            return;
        Time.timeScale = 0;
        AudioListener.pause = true;
        gameOverScreen.SetActive(true);
        var _gameOver = gameOverScreen.GetComponent<GameOverScreen>();
        _gameOver.SetScoreText((int)_gameTime,_lives,_coins,_kills);
    }

    private void ResetGameState()
    {
        _gameTime = 0;
        _lives = startingLives;
        _coins = 0;
        _kills = 0;
        OnCoinsChanged?.Invoke(_coins);
        OnLivesChanged?.Invoke(_lives);

        _currentGame = GetComponentInChildren<LevelsAndPlayer>();
        _currentPlayer = _currentGame.Player;
        _currentPlayerTransform = _currentPlayer.transform;
        _currentLevelManager = _currentGame.LevelManager;
        OnPlayerCloned?.Invoke(_currentPlayer.PlayerHealthController);
        _currentPlayer.PlayerHealthController.ResetHealth();
        

        if (gameOverScreen == null)
            return;
        var _gameOver = gameOverScreen.GetComponent<GameOverScreen>();
        _gameOver.GetPlayerInput(CurrentPlayer.InputHandler);

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
