using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/ReturnTrue_SO", fileName = "ReturningTrue_SO", order = 0)]
public class ReturnTrueDecision_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        return true;
    }
}
