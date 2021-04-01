using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour, ITakeSpikeDamage
{
    [SerializeField] private float takeDamageCooldownTime;
    
    public bool TookDamage { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    
    private int _currentHealth;
    private float _lastTimePlayerTookDamage;

    private void Awake()
    {
        MaxHealth = 5;
        _currentHealth = MaxHealth;
        _lastTimePlayerTookDamage = 0;
    }

    public void ResetTookDamage() => TookDamage = false;

    public void TakeDamage()
    {
        if (Time.time < _lastTimePlayerTookDamage + takeDamageCooldownTime)
            return;
        
        CurrentHealth--;
        TookDamage = true;
        _lastTimePlayerTookDamage = Time.time;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("mamma mia!");
    }

    public void TakeSpikeDamage()
    {
        TakeDamage();
    }
    
}
