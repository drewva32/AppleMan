using UnityEngine;

[CreateAssetMenu(menuName = "Create State/ImmovableState_SO", fileName = "ImmovableState_SO", order = 0)]
public class ImmovableState_SO : State
{
    public override void OnEnter(PluggableStateController controller)
    {
        base.OnEnter(controller);
        controller.Enemy.AllowDamage(false);
        controller.WalkingController.RB.bodyType = RigidbodyType2D.Static;
    }

    public override void OnExit(PluggableStateController controller)
    {
        base.OnExit(controller);
        controller.Enemy.AllowDamage(true);
        controller.WalkingController.RB.bodyType = RigidbodyType2D.Dynamic;
    }
}
