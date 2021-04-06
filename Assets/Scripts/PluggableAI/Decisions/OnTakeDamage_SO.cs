using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTakeDamage_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        //bool hasTakenDamage = controller.EnemyHealthController.HasTakenDamage;
        //controller.EnemyHealthController.ResetHasTakenDamage;
        //return hasTakenDamage;
        return false;//remove this
    }
}
