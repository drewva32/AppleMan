using System;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    [SerializeField]
    private int _hitPoints = 2;
    [SerializeField]
    private float _meleeDistance = 0.5f;
    [SerializeField]
    private float _dashSpeed = 4f;
    [SerializeField]
    private Transform _meleeVector;

    public float MeleeDistance => _meleeDistance;
    public Transform MeleeChecker => _meleeVector;
    public bool IsChargeComplete => _chargeComplete;

    public float DashSpeed => _dashSpeed;

    private bool _chargeComplete = false;

    private void Start()
    {
        SetupStats(true, _hitPoints);
        SetCharge(false);
    }

    private void OnDrawGizmos()
    {
        // Draw circle for Melee
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_meleeVector.position, _meleeDistance);
    }

    public void SetCharge(bool isChargeComplete)
    {
        _chargeComplete = isChargeComplete;
    }

}
