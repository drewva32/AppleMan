using UnityEngine;


[CreateAssetMenu(menuName = "Create Decision/InstaKillDecision_SO", fileName = "InstaKill_SO", order = 0)]
public class IntsaKillDecision_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        bool isDead = controller.GetComponent<EnemyBase>().CurrentHealth > 0;
        return isDead;
    }
}
