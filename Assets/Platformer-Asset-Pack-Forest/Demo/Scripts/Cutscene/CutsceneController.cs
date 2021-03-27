using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    public class CutsceneController : Singleton<CutsceneController>
    {
        public void PlayLiftObjectCutscene(Player2D p_player, Liftable2D p_liftingObject)
        {
            StartCoroutine(LiftObjectCutscene(p_player, p_liftingObject));
        }

        private IEnumerator LiftObjectCutscene(Player2D p_player, Liftable2D p_liftingObject)
        {
            p_player.updateInput = false;
            p_player.updateAnimations = false;
            p_player.animator.Play("run");

            Vector2 pickPivot = p_player.position.x > p_liftingObject.position.x ? p_liftingObject.pickupPivotRight.position : p_liftingObject.pickupPivotLeft.position;

            p_player.scaleX = p_player.position.x > p_liftingObject.position.x ? -1 : 1;

            while (Vector2.Distance(p_player.position, pickPivot) > 0.1f)
            {
                p_player.position = Vector2.MoveTowards(p_player.position, pickPivot, p_player.moveSpeed * Time.deltaTime);
            }

            p_player.position = pickPivot;
            p_liftingObject.transform.SetParent(p_player.transform);
            p_player.animator.PlayWithOnKeyFrameEvent("pick-up", p_player.OnLiftObject);

            yield return new WaitForSeconds(p_player.animator.GetCurrentAnimationLength());

            p_player.animator.Play("idle-with-item");
            p_player.updateInput = true;
            p_player.updateAnimations = true;
        }

        public void PlayThrowObjectCutscene(Player2D p_player, Liftable2D p_throwingObject)
        {
            StartCoroutine(ThrowObjectCutscene(p_player, p_throwingObject));
        }

        private IEnumerator ThrowObjectCutscene(Player2D p_player, Liftable2D p_throwingObject)
        {
            p_player.updateInput = false;
            p_player.updateAnimations = false;
            p_player.animator.PlayWithOnKeyFrameEvent("throw", p_player.OnThrowObject);

            yield return new WaitForSeconds(p_player.animator.GetCurrentAnimationLength());

            p_player.animator.Play("idle");
            p_player.updateInput = true;
            p_player.updateAnimations = true;
        }
    }
}
