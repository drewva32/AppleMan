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
    
    private int _currentHealth;
    private float _lastTimePlayerTookDamage;
    private SpriteInvulnerableFlash _spriteInvulnerableFlash;

    private void Awake()
    {
        _spriteInvulnerableFlash = GetComponent<SpriteInvulnerableFlash>();
        MaxHealth = 5;
        _currentHealth = MaxHealth;
        _lastTimePlayerTookDamage = 0;
    }

    public void ResetTookDamage() => TookDamage = false;

    public void TakeDamage()
    {
        if (Time.time < _lastTimePlayerTookDamage + takeDamageCooldownTime)
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

    private IEnumerator WaitThenFlashRoutine()
    {
        yield return new WaitForSeconds(7 / 12f);
        _spriteInvulnerableFlash.FlashSprite();
    }

    private void Die()
    {
        Debug.Log("mamma mia!");
        OnDie?.Invoke();
    }
    

    public void TakeSpikeDamage()
    {
        TakeDamage();
    }
    
}
