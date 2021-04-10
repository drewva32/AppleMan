using UnityEngine;


[CreateAssetMenu(menuName = "Create State/MeleeChargeState_SO", fileName = "MeleeChargeState_SO", order = 0)]
public class MeleeChargeState_SO : State
{
    public override void OnEnter(PluggableStateController controller)
    {
        base.OnEnter(controller);
        controller.WalkingController.SetVelocity(Vector2.zero);
    }

    public override void OnExit(PluggableStateController controller)
    {
        base.OnExit(controller);
    }
}
