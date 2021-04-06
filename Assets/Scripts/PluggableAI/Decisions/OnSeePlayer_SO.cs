using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/OnSeePlayer_SO", fileName = "OnSeePlayer_SO", order = 0)]
public class OnSeePlayer_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        bool canSeePlayer = Physics2D.Raycast(controller.WalkingController.WallCheck.position,
            controller.WalkingController.vectorDirection, controller.WalkingController.ChaseDistance, controller.WalkingController.PlayerLayer);
        return canSeePlayer;
    }
}


