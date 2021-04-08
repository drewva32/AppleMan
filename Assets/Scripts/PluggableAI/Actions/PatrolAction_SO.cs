using UnityEngine;

[CreateAssetMenu(menuName = "Create Action/PatrolAction_SO", fileName = "PatrolAction_SO", order = 0)]
public class PatrolAction_SO : StateAction
{
    public override void LogicUpdate(PluggableStateController controller)
    {
        controller.WalkingController.CheckDirection();
    }

    public override void FixedLogicUpdate(PluggableStateController controller)
    {
        controller.WalkingController.Walk();
    }
}
