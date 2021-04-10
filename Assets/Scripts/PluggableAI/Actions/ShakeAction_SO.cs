using UnityEngine;

[CreateAssetMenu(menuName = "Create Action/ShakeAction_SO", fileName = "ShakeAction_SO", order = 0)]
public class ShakeAction_SO : StateAction
{
    private float _shakeDuration = 1.0f;
    // A measure of magnitude for the shake 
    private float _shakeAmount = 0.2f;

    public override void FixedLogicUpdate(PluggableStateController controller)
    {
        
    }

    public override void LogicUpdate(PluggableStateController controller)
    {
        if (_shakeDuration > 0)
        {
            Vector3 newPos = Random.insideUnitSphere * (Time.deltaTime * _shakeAmount);
            newPos.y = controller.WalkingController.Melee.transform.localPosition.y;
            newPos.z = controller.WalkingController.Melee.transform.localPosition.z;
            controller.WalkingController.Melee.transform.localPosition = newPos;
            _shakeDuration -= Time.deltaTime;
        }
        else
        {
            controller.WalkingController.Melee.SetCharge(true);
            _shakeDuration = 1.0f;
        }
    }
}
