using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    private EnemyStateController _stateController;
    private EnemyBase enemy;
    private PluggableStateController _pluggableStateController;

    private void Awake()
    {
        _pluggableStateController = GetComponentInParent<PluggableStateController>();
        
        enemy = GetComponentInParent<EnemyBase>();
    }

    public void AnimationTriggerFinish()
    {
        _pluggableStateController.OnAnimationEnd();
    }

    public void LaunchProjectile()
    {
        enemy.LaunchProjectile();
    }

    public void FinishDeath()
    {
        Destroy(this.transform.parent.gameObject);
    }

    public void DestroyGameObjct()
    {
        Destroy(this.gameObject);
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
