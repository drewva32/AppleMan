using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/IsPlayerOutOfSight_SO", fileName = "IsPlayerOutOfSight_SO", order = 0)]
public class IsPlayerOutOfSight_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        bool canSeePlayer = Physics2D.Raycast(controller.WalkingController.WallCheck.position,
            controller.WalkingController.vectorDirection, controller.WalkingController.VisionDistance, controller.WalkingController.PlayerLayer) == false;
        return canSeePlayer;
    }
}
