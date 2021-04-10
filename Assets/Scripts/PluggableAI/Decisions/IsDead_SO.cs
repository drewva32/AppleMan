using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/IsDeadDecision_SO", fileName = "IsDeadDecision_SO", order = 0)]
public class IsDead_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        bool isDead = controller.GetComponent<EnemyBase>().CurrentHealth <= 0;
        return isDead;
    }
}
