using UnityEngine;

public class EnemyBase : MonoBehaviour, IPlayerInteractions
{
    public bool _canTakeDamage { get; private set; }
    public int CurrentHealth => _currentHealth;
    public bool HasTakenDamage => _hasTakenDamage;

    private WalkingController _walkingController;
    private int _currentHealth;
    private bool _hasTakenDamage = false;

    

    private void Start()
    {
        _walkingController = GetComponent<WalkingController>();
    }

    public void TakeDamage(int amount)
    {
        if (_canTakeDamage)
        {
            _hasTakenDamage = true;
            _canTakeDamage = false;
            _currentHealth -= amount;
        }
    }

    public virtual void LaunchProjectile()
    { }

    public void TakePunch(int damageAmount)
    {
        TakeDamage(damageAmount);
    }

    public void TakeSlide(int damageAmount, Vector3 directionToPlayer)
    {
        TakeDamage(damageAmount);
        _walkingController.RB.AddForce(new Vector2(-directionToPlayer.x * 100, 0));
    }

    public void ResetHasTakenDamage()
    {
        _hasTakenDamage = false;
        _canTakeDamage = true;
    }

    public void SetupStats(bool canTakeDamage, int health)
    {
        _canTakeDamage = canTakeDamage;
        _currentHealth = health;
    }

    public void AllowDamage(bool caTakeDamage)
    {
        _canTakeDamage = caTakeDamage;
    }
}
