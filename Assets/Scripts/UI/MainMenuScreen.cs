using System;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    private Player _player;
    private bool _dashPressed;
    private bool _punchPressed;
    private bool _initialized;

    private void Start()
    {
        _player = AppleGameManager.Instance.CurrentPlayer;
        _initialized = true;
    }

    private void OnEnable()
    {
        if (_initialized)
            _player = AppleGameManager.Instance.CurrentPlayer;
    }

    private void Update()
    {
        _dashPressed = _player.InputHandler.DashInput;
        _punchPressed = _player.InputHandler.PunchInput;

        if (_dashPressed || _punchPressed)
        {
            
            gameObject.SetActive(false);
            AppleGameManager.Instance.StartGame();
        }
    }
}
