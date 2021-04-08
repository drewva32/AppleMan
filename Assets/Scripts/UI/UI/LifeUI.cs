using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    
    private void Start()
    {
        if(AppleGameManager.Instance != null)
            AppleGameManager.Instance.OnLivesChanged += UpdateLifeUI;
    }
    // private void OnDisable() => AppleGameManager.Instance.OnCoinsChanged -= UpdateCoinUI;

    private void UpdateLifeUI(int lives)
    {
        coinText.text = lives.ToString("N0");
    }
}
