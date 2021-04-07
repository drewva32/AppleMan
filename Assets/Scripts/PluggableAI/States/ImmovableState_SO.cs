using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create State/ ImmovableState_SO", fileName = "ImmovableState_SO", order = 0)]
public class ImmovableState_SO : State
{
    private float gravScale;

    public override void OnEnter(PluggableStateController controller)
    {
        base.OnEnter(controller);
        gravScale = controller.WalkingController.RB.gravityScale;
        controller.WalkingController.RB.gravityScale = 100;
    }

    public override void OnExit(PluggableStateController controller)
    {
        base.OnExit(controller);
        controller.WalkingController.RB.gravityScale = gravScale;
    }
}
