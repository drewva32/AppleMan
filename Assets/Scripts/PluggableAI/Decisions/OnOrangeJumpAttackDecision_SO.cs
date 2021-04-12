using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/OnOrangeJumpAttackDecision_SO", fileName = "OnOrangeJumpAttackDecision_SO", order = 0)]
public class OnOrangeJumpAttackDecision_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        return controller.GetComponent<OrangeBoss>().CurrentAttack == "jump";
    }
}
