using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/IsEnemyOnPlatform_SO", fileName = "IsEnemyOnPlatform_SO", order = 0)]
public class IsEnemyOnPlatform_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        return controller.WalkingController.OnPlatform;
    }
}
