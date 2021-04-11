using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteInvulnerableFlash))]
public class PlayerHealthController : MonoBehaviour, ITakeSpikeDamage
{
    [SerializeField] private float takeDamageCooldownTime;

    public event Action<int> OnHealthChanged;
    public event Action OnDie;
    public bool TookDamage { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    public bool CanTakeDamage { get; set; }
    
    private int _currentHealth;
    private float _lastTimePlayerTookDamage;
    private SpriteInvulnerableFlash _spriteInvulnerableFlash;

    private void Awake()
    {
        _spriteInvulnerableFlash = GetComponent<SpriteInvulnerableFlash>();
        MaxHealth = 5;
        _currentHealth = MaxHealth;
        _lastTimePlayerTookDamage = 0;
        CanTakeDamage = true;
    }

    public void ResetTookDamage() => TookDamage = false;

    public void TakeDamage()
    {
        if (Time.time < _lastTimePlayerTookDamage + takeDamageCooldownTime || !CanTakeDamage)
            return;
        Debug.Log("took dammage");

        CurrentHealth--;
        OnHealthChanged?.Invoke(_currentHealth);
        TookDamage = true;
        _lastTimePlayerTookDamage = Time.time;
        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(WaitThenFlashRoutine());
        }
    }

    public void Heal()
    {
        CurrentHealth++;
        if (CurrentHealth > 5)
            CurrentHealth = 5;
        OnHealthChanged?.Invoke(_currentHealth);
    }

    private IEnumerator WaitThenFlashRoutine()
    {
        yield return new WaitForSeconds(7 / 12f);
        _spriteInvulnerableFlash.FlashSprite();
    }

    public void Die()
    {
        Debug.Log("mamma mia!");
        OnDie?.Invoke();
        if(AppleGameManager.Instance!=null)
            AppleGameManager.Instance.ChangeLives(false);
        CurrentHealth = 5;
        OnHealthChanged?.Invoke(_currentHealth);
    }
    

    public void TakeSpikeDamage()
    {
        TakeDamage();
    }
    
}
