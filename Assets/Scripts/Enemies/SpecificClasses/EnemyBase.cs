using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth;

    public int CurrentHealth => _currentHealth;
    private AnimationHelper animHelper;
    private int _currentHealth;

    private void Start()
    {
        animHelper = GetComponentInChildren<AnimationHelper>();

        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
    }

    public virtual void LaunchProjectile()
    { }
}
