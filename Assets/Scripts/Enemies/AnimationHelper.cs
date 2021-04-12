using System.Collections;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    private EnemyBase enemy;
    private PluggableStateController _pluggableStateController;
    private GameObject _parent;

    private void Awake()
    {
        _pluggableStateController = GetComponentInParent<PluggableStateController>();
        
        enemy = GetComponentInParent<EnemyBase>();
        if(enemy != null)
            _parent = gameObject.transform.parent.gameObject;
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
        _parent.SetActive(false);
    }

    public void DeactivateGameObject()
    {
        _parent.SetActive(false);
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
