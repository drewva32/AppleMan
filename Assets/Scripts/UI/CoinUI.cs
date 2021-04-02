using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    
    private void Start()
    {       
        if(AppleGameManager.Instance != null)
            AppleGameManager.Instance.OnCoinsChanged += UpdateCoinUI;
    }
    // private void OnDisable() => AppleGameManager.Instance.OnCoinsChanged -= UpdateCoinUI;

    private void UpdateCoinUI(int coins)
    {
        coinText.text = coins.ToString("N0");
    }
}
