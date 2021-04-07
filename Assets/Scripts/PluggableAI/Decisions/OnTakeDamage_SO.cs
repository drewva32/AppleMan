using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/OnTakeDamage_SO", fileName = "OnTakeDamage_SO", order = 0)]
public class OnTakeDamage_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        bool hasTakenDamage = controller.Enemy.HasTakenDamage;
        controller.Enemy.ResetHasTakenDamage();
        return hasTakenDamage;
    }
}
