using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    private EnemyStateController _stateController;
    private EnemyBase enemy;

    private void Awake()
    {
        _stateController = GetComponentInParent<EnemyStateController>();
        enemy = GetComponentInParent<EnemyBase>();
    }

    public void AnimationTriggerFinish()
    {
        _stateController.AnimationTrigger();
    }

    public void LaunchProjectile()
    {
        enemy.LaunchProjectile();
    }
}
