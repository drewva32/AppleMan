using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/IsPlayerOutOfProtectionRange_SO", fileName = "IsPlayerOutOfProtectionRange_SO", order = 0)]
public class IsPlayerOutOfProtectionRange_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        bool isPlayerClose = Physics2D.Raycast(controller.WalkingController.WallCheck.position,
            controller.WalkingController.vectorDirection, controller.WalkingController.ProtectDistance, controller.WalkingController.PlayerLayer) == false;
        return isPlayerClose;
    }
}
