using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    private EnemyStateController _stateController;

    private void Awake()
    {
        _stateController = GetComponentInParent<EnemyStateController>();
    }

    public void AnimationTriggerFinish()
    {
        _stateController.AnimationTrigger();
    }
}
