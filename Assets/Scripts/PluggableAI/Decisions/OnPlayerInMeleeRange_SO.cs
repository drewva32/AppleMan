using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/IsPlayerInRange_SO", fileName = "IsPlayerInRange_SO", order = 0)]
public class OnPlayerInMeleeRange_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        bool canPunchPlayer = Physics2D.Raycast(controller.WalkingController.WallCheck.position, controller.WalkingController.vectorDirection, controller.WalkingController.Melee.MeleeDistance, controller.WalkingController.PlayerLayer);
        bool isPlayerClose = Physics2D.Raycast(controller.WalkingController.WallCheck.position,
            controller.WalkingController.vectorDirection, controller.WalkingController.Melee.MeleeDistance, controller.WalkingController.PlayerLayer);
        return canPunchPlayer;
    }
}
