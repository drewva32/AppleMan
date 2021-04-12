using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/OnOrangeChargeAttackDecision_SO", fileName = "OnOrangeChargeAttackDecision_SO", order = 0)]
public class OnOrangeChargeAttackDecision_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        return controller.GetComponent<OrangeBoss>().CurrentAttack == "charge";
    }
}
