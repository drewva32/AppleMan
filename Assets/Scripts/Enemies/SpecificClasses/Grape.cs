using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : EnemyBase
{
    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private Transform _throwPosition;
    [SerializeField]
    private Transform _projectileParent;

    public override void LaunchProjectile()
    {
        GameObject graplette = Instantiate(_projectile, _throwPosition.position, transform.rotation);
        graplette.transform.parent = _projectileParent.transform;
    }
}
