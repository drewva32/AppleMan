using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : EnemyBase
{
    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private Transform _throwPosition;
    private Transform _projectileParent;

    private void Start()
    {
        _projectileParent = GameObject.Find("Projectiles").transform;
    }

    public override void LaunchProjectile()
    {
        GameObject graplette = Instantiate(_projectile, _throwPosition.position, transform.rotation);
        graplette.transform.parent = _projectileParent.transform;
    }
}
