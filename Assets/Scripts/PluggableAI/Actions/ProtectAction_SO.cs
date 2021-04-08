using UnityEngine;

[CreateAssetMenu(menuName = "Create Action/ ProtectAction_SO", fileName = "ProtectAction_SO", order = 0)]
public class ProtectAction_SO : StateAction
{
    public override void FixedLogicUpdate(PluggableStateController controller)
    {
        
    }

    public override void LogicUpdate(PluggableStateController controller)
    {
        controller.Enemy.AllowDamage(false);
    }
}
