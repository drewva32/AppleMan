using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create State/RangedAttackState_SO", fileName = "RangedAttackState_SO", order = 0)]
public class RangedAttackState_SO : State
{
    public override void OnEnter(PluggableStateController controller)
    {
        base.OnEnter(controller);
        controller.WalkingController.RB.velocity = Vector2.zero;
    }

    public override void OnExit(PluggableStateController controller)
    {
        base.OnExit(controller);
    }
}
