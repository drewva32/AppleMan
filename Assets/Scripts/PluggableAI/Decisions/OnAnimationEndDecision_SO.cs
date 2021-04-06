using UnityEngine;

[CreateAssetMenu(menuName = "Create Decision/ OnAnimationEndDecision_SO", fileName = "OnAnimationEndDecision_SO", order = 0)]
public class OnAnimationEndDecision_SO : StateDecision
{
    public override bool Condition(PluggableStateController controller)
    {
        bool animationEnded = controller.IsAnimationComplete;
        controller.ResetAnimationComplete();
        return animationEnded;
    }
}
