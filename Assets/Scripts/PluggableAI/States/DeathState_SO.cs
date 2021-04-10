using UnityEngine;

[CreateAssetMenu(menuName = "Create State/DeathState_SO", fileName = "DeathState_SO", order = 0)]
public class DeathState_SO : State
{
    public override void OnEnter(PluggableStateController controller)
    {
        base.OnEnter(controller);
        controller.WalkingController.Collider.enabled = false;
    }

    public override void OnExit(PluggableStateController controller)
    {
        base.OnExit(controller);
    }
}
