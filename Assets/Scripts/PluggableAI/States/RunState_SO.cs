using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create State/RunState_SO", fileName = "RunState_SO", order = 0)]
public class RunState_SO : State
{
    [SerializeField]
    private float runSpeed = 2.5f;

    private float currentWalkSpeed;
    public override void OnEnter(PluggableStateController controller)
    {
        base.OnEnter(controller);
        currentWalkSpeed = controller.WalkingController.Speed;
        controller.WalkingController.ChangeWalkSpeed(runSpeed);
    }

    public override void OnExit(PluggableStateController controller)
    {
        base.OnExit(controller);
        controller.WalkingController.ChangeWalkSpeed(currentWalkSpeed);
    }
}
