using UnityEngine;

[CreateAssetMenu(menuName = "Create State/WillowMoveState_SO", fileName = "WillowMoveState_SO", order = 0)]
public class WillowMoveState_SO : State
{
    public override void OnEnter(PluggableStateController controller)
    {
        base.OnEnter(controller);
        controller.GetComponent<DrWillow>().Test();
    }

    public override void OnExit(PluggableStateController controller)
    {
        base.OnExit(controller);
    }
}
