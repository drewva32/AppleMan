using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IPlayerInteractions
{
    [SerializeField]
    private int _maxHealth;

    public int CurrentHealth => _currentHealth;
    public bool HasTakenDamage => _hasTakenDamage;
    private AnimationHelper _animHelper;
    private WalkingController _walkingController;
    private int _currentHealth;
    private bool _hasTakenDamage = false;

    private void Start()
    {
        _animHelper = GetComponentInChildren<AnimationHelper>();
        _walkingController = GetComponent<WalkingController>();

        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
    }

    public virtual void LaunchProjectile()
    { }

    public void TakePunch(int damageAmount)
    {
        _hasTakenDamage = true;
        _walkingController.RB.velocity = Vector2.zero;
        // Apply damage
        TakeDamage(damageAmount);
        CheckHealth();
    }

    public void TakeSlide(int damageAmount, Vector3 directionToPlayer)
    {
        // Apply Damage
        _hasTakenDamage = true;
        TakeDamage(damageAmount);
        CheckHealth();
        _walkingController.RB.AddForce(new Vector2(-directionToPlayer.x * 250, 0));
        // Change to hit state
    }

    private void CheckHealth()
    {
        if (_currentHealth <= 0)
        {
            // Change state to die state
        }
    }

    public void ResetHasTakenDamage()
    {
        _hasTakenDamage = false;
    }
}
