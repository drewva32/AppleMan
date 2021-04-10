using UnityEngine;

[CreateAssetMenu(menuName = "Create Action/ChargeAttackAction_SO", fileName = "ChargeAttackAction_SO", order = 0)]
public class ChargeAttackAction_SO : StateAction
{
    public override void FixedLogicUpdate(PluggableStateController controller)
    {
        controller.WalkingController.SetVelocity(Vector2.right * controller.WalkingController.vectorDirection * controller.WalkingController.Melee.DashSpeed);
    }

    public override void LogicUpdate(PluggableStateController controller)
    {
        
    }
}
