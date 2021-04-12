using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create State/BossBattleState_SO", fileName = "BossBattleState_SO", order = 0)]
public class BossBattleState_SO : State
{
    public override void OnEnter(PluggableStateController controller)
    {
        base.OnEnter(controller);
        //controller.GetComponent<OrangeBoss>().MakeDecision(Random.Range(0.2f, 1f));
    }

    public override void OnExit(PluggableStateController controller)
    {
        base.OnExit(controller);
    }
}
