using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuText;
    
    private PlayerInputHandler _inputHandler;
    private bool _hasGottenPlayerInput;
    private float _startTime;
    private bool _dashInput;
    private bool _punchInput;
    
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
        if (Time.unscaledTime < _startTime + 5f || !_hasGottenPlayerInput)
            return;
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
}
