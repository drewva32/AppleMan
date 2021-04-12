using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/WillowMoveDecision_SO", fileName = "WillowMoveDecision_SO", order = 0)]
public class WillowMoveDecision_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        return controller.GetComponent<DrWillow>().DecisionMade;
    }
}
