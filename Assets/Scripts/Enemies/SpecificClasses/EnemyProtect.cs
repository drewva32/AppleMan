using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProtect : EnemyBase
{
    [SerializeField]
    private int _hitPoints = 2;
    [SerializeField]
    private float _protectStartDistance = 0.2f;

    public float ProtectDistance => _protectStartDistance;

    private void Start()
    {
        SetupStats(true, _hitPoints);
    }
}
