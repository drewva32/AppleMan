using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/IsChargeComplete", fileName = "IsChargeComplete", order = 0)]
public class IsChargeComplete : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        return controller.WalkingController.Melee.IsChargeComplete;
    }
}
