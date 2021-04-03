using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private PlayerHealthController _playerHealthController;
    private Image _healthBar;

    private void Awake()
    {
        _playerHealthController = FindObjectOfType<PlayerHealthController>();
        _healthBar = GetComponent<Image>();
    }

    private void Start()
    {
        _playerHealthController.OnHealthChanged += UpdateHealthBarFill;
    }

    private void UpdateHealthBarFill(int currentHealth)
    {
        _healthBar.fillAmount = ((float)currentHealth / 5f) - (0.05f * (float)currentHealth/5f);
    }
}
