using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyBase
{
    [SerializeField]
    private int _hitPoints = 2;
    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private Transform _throwPosition;
    private Transform _projectileParent;

    private void Start()
    {
        _projectileParent = GameObject.Find("Projectiles").transform;
        SetupStats(true, _hitPoints);
    }

    public override void LaunchProjectile()
    {
        GameObject graplette = Instantiate(_projectile, _throwPosition.position, transform.rotation);
        graplette.transform.parent = _projectileParent.transform;
    }
}
