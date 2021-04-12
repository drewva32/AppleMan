using UnityEngine;

[CreateAssetMenu(menuName = "Create Action/OrangeFocusAction_SO", fileName = "OrangeFocusAction_SO", order = 0)]
public class OrangeFocusAction_SO : StateAction
{
    private float timer = 0;
    public override void FixedLogicUpdate(PluggableStateController controller)
    {
        
    }

    public override void LogicUpdate(PluggableStateController controller)
    {
        if (timer < 0.5f)
            controller.GetComponent<OrangeBoss>().FocusAttack();
    }
}
