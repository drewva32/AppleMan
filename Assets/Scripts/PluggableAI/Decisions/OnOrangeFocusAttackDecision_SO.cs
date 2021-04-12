using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/OnOrangeFocusAttackDecision_SO", fileName = "OnOrangeFocusAttackDecision_SO", order = 0)]
public class OnOrangeFocusAttackDecision_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        return controller.GetComponent<OrangeBoss>().CurrentAttack == "focus";
    }
}
