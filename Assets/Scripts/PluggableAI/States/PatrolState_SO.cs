using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create State/ PatrolState_SO", fileName = "PatrolState_SO", order = 0)]
public class PatrolState_SO : State
{
    public override void OnEnter(PluggableStateController controller)
    {
        base.OnEnter(controller);
        // do special stuff onentering patrol state
    }

    public override void OnExit(PluggableStateController controller)
    {
        base.OnExit(controller);
        //do special stuff onexiting patrol state
    }
}
