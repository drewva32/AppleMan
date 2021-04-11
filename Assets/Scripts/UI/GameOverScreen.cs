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

        rankAndDescriptionText.text = GetRating(score);
    }

    private int CalculateScore(int time, int lives, int coins, int enemies)
    {
        int finalScore = 0;
        if (lives > 0)
            finalScore = -time * 3;
        finalScore += (lives * 1000);
        finalScore += (coins * 6);
        finalScore += (enemies * 90);
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

    public string GetRating(int score)
    {
        if (score > 8000)
            return "Apple of Our Eye - The fruits of your labor paid off!";
        else if (score > 6000)
            return "The big apple - Almost to the top!";
        else if (score > 2000)
            return "Honeycrisp - How ya like them apples?!";
        else if (score > 2000)
            return "Bruised but not Broken - perhaps we could put you in a pie?";
        else if (score > 1000)
            return "Applesauce - Try to be more crisp next time!";
        else if (true)
            return "Crabapple - Try to be more crisp next time!";
    }
}
