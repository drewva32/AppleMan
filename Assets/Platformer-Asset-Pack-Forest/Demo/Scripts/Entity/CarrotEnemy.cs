using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    public class CarrotEnemy : Enemy2D
    {
        public float m_moveSpeed;
        public Vector2[] localWaypoints;

        private int m_fromWaypointIndex;
        private float m_percentBetweenWaypoints;
        private Vector2[] globalWaypoints;
        private Vector2 m_velocity;

        private bool m_updateAnimations;
        private bool m_updateMovement;

        private Hurtbox m_hurtBox;
        private Hitbox m_hitBox;

        public override void Start()
        {
            base.Start();

            m_updateAnimations = true;
            m_updateMovement = true;

            globalWaypoints = new Vector2[localWaypoints.Length];

            m_hurtBox = GetComponentInChildren<Hurtbox>();
            m_hitBox = GetComponentInChildren<Hitbox>();

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                globalWaypoints[i] = localWaypoints[i] + (Vector2)transform.position;
            }
        }

        public override void Update()
        {
            if (m_updateMovement)
            {
                if (m_animator.currentAnimationName.Equals("move") && m_animator.CurrentFrame > 2 && m_animator.CurrentFrame < 8)
                {
                    m_velocity = GetDirectionToNextWaypoint();
                    m_controller.Move(m_velocity);
                }

                scaleX = -Mathf.Sign(m_velocity.x);
            }

            if (m_updateAnimations)
            {
                UpdateAnimations();
            }
        }

        public void UpdateAnimations()
        {
            //if (Mathf.Abs(m_velocity.x) > 0)
            //{
            //    PlayAnimation("move");
            //}
            //else
            //{
            //    PlayAnimation("idle");
            //}

            PlayAnimation("move");
        }

        private Vector2 GetDirectionToNextWaypoint()
        {
            m_fromWaypointIndex %= globalWaypoints.Length;

            int toWaypointIndex = (m_fromWaypointIndex + 1) % globalWaypoints.Length;
            float distanceBetweenWaypoints = Vector2.Distance(globalWaypoints[toWaypointIndex], globalWaypoints[m_fromWaypointIndex]);
            m_percentBetweenWaypoints += Time.deltaTime * m_moveSpeed / distanceBetweenWaypoints;

            Vector2 newPos = Vector2.Lerp(globalWaypoints[m_fromWaypointIndex], globalWaypoints[toWaypointIndex], m_percentBetweenWaypoints);

            if (m_percentBetweenWaypoints >= 1)
            {
                m_percentBetweenWaypoints = 0;
                m_fromWaypointIndex++;

                if (m_fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    m_fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }

            return newPos - (Vector2)transform.position;
        }

        public override void OnDamaged(DamageInfo p_damageInfo)
        {
            m_updateAnimations = false;
            m_updateMovement = false;
            base.OnDamaged(p_damageInfo);
        }

        protected override void OnDamageComplete()
        {
            m_updateAnimations = true;
            m_updateMovement = true;
            base.OnDamageComplete();
        }

        public override void OnDeath(DamageInfo p_damageInfo)
        {
            m_updateAnimations = false;
            m_updateMovement = false;
            m_hitBox.gameObject.SetActive(false);
            m_hurtBox.gameObject.SetActive(false);
            m_health.currentValue = 0;

            base.OnDeath(p_damageInfo);
        }

        protected override void OnDeathComplete()
        {
            base.OnDeathComplete();
        }

        private void OnDrawGizmos()
        {
            if (localWaypoints != null)
            {
                Gizmos.color = Color.red;
                float size = 0.25f;

                for (int i = 0; i < localWaypoints.Length; i++)
                {
                    Vector2 globalWaypoint = Application.isPlaying ? globalWaypoints[i] : localWaypoints[i] + (Vector2)transform.position;
                    Gizmos.DrawLine(globalWaypoint - Vector2.up * size, globalWaypoint + Vector2.up * size);
                    Gizmos.DrawLine(globalWaypoint - Vector2.left * size, globalWaypoint + Vector2.left * size);
                }
            }
        }

    }
}
