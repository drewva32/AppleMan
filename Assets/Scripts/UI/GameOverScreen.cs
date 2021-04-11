using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuText;

    [Header("Text")] 
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI rankAndDescriptionText;
    
    private PlayerInputHandler _inputHandler;
    private bool _hasGottenPlayerInput;
    private float _startTime;
    private bool _dashInput;
    private bool _punchInput;

    public void SetScoreText(int time, int lives, int coins, int enemies)
    {
        timeText.text = time.ToString("N0");
        livesText.text = lives.ToString();
        coinsText.text = coins.ToString();
        killsText.text = enemies.ToString();
        
        int score = CalculateScore(time,lives,coins,enemies);
        finalScoreText.text = score.ToString("N0");

        rankAndDescriptionText.text = "CrabApple - Is That All YOU Got Bro?!";
    }

    private int CalculateScore(int time, int lives, int coins, int enemies)
    {
        int finalScore = -time;
        finalScore += (lives * 1000);
        finalScore += (coins * 6);
        finalScore += (enemies * 200);
        return finalScore;
    }

    public void GetPlayerInput(PlayerInputHandler inputHandler)
    {
        _inputHandler = inputHandler;
        _hasGottenPlayerInput = true;
    }

    private void OnEnable()
    {
        _startTime = Time.unscaledTime;
    }

    private void Update()
    {
        if (!_hasGottenPlayerInput)
            return;
        if (Time.unscaledTime < _startTime + 5f)
        {
            _inputHandler.UseDashInput();
            _inputHandler.UseJumpInput();
            return;
        }
        
        // Debug.Log("has gottenplayerinput");
        _dashInput = _inputHandler.DashInput;
        _punchInput = _inputHandler.PunchInput;
        mainMenuText.SetActive(true);
        if (_dashInput || _punchInput)
        {
            mainMenuText.SetActive(false);
            // Debug.Log("input worked");
            gameObject.SetActive(false);
            _inputHandler.UseDashInput();
            _inputHandler.UsePunchInput();
            AppleGameManager.Instance.MainMenu();
        }
    }

    public string GetRating(int Score)
    {
        return "hi";
    }
}
