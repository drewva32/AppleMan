using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGameManager : MonoBehaviour
{
    private static AppleGameManager _instance;
    public static AppleGameManager Instance => _instance;

    private int _coins;
    private int _kills;
    private int _lives;
    
    public event Action<int> OnLivesChanged;
    public event Action<int> OnCoinsChanged;

    private void Awake()
    {
        if(_instance != null)
            Destroy(gameObject);
        else
        {
            _instance = this;
        }
    }
    private void OnDestroy() => _instance = null;

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
        OnLivesChanged?.Invoke(_lives);
    }
}
