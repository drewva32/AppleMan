using UnityEngine;

[CreateAssetMenu(menuName = "Create Action/OrangeChargeAction_SO", fileName = "OrangeChargeAction_SO", order = 0)]
public class OrangeChargeAction_SO : StateAction
{
    public override void FixedLogicUpdate(PluggableStateController controller)
    {
        controller.WalkingController.SetVelocity(Vector2.right * controller.WalkingController.vectorDirection * 10);
    }

    public override void LogicUpdate(PluggableStateController controller)
    {
        
    }
}
