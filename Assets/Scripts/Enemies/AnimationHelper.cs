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

    public void FinishDeath()
    {
        Destroy(this.transform.parent.gameObject);
    }

    public void  Wait(float waitTime)
    {
        StartCoroutine(WaitForSeconds(waitTime));
    }

    IEnumerator WaitForSeconds(float waitTime)
    {
        while (waitTime > 0)
        {
            yield return new WaitForSeconds(1f);
            waitTime--;
        }
    }
}
