using UnityEngine;

[CreateAssetMenu(menuName = "Create Action/MeleeAction_SO", fileName = "MeleeAction_SO", order = 0)]
public class MeleeAction_SO : StateAction
{
    public override void FixedLogicUpdate(PluggableStateController controller)
    {
        // Stop velocity 
        controller.WalkingController.RB.velocity = Vector2.zero;
    }

    public override void LogicUpdate(PluggableStateController controller)
    {
        // Check Punch radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(controller.WalkingController.Melee.transform.position, controller.WalkingController.Melee.MeleeDistance, controller.WalkingController.PlayerLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].tag == "Player")
            {
                colliders[i].GetComponent<PlayerHealthController>().TakeDamage();
                break;
            }
        }
    }
}
