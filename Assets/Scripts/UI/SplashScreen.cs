using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private Player _player;
    private bool _anyKeyPressed;

    private void Start()
    {
        _player = AppleGameManager.Instance.CurrentPlayer;
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    private void Update()
    {
        _anyKeyPressed = _player.InputHandler.AnyKeyPressedInput;

        if (_anyKeyPressed)
        {
            gameObject.SetActive(false);
            AppleGameManager.Instance.MainMenu();
        }
    }
}