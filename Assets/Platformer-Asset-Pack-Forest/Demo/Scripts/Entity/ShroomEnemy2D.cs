using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    public class ShroomEnemy2D : Enemy2D
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

        private bool m_protected;

        public override bool canJumpOnHead
        {
            get { return !m_protected; }
        }

        // protect-start
        // protect-end


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

            PlayAnimation("move");
        }

        public override void Update()
        {
            HandleProtect();

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
            if (Mathf.Abs(m_velocity.x) > 0)
            {
                PlayAnimation("move");
            }
        }

        private void HandleProtect()
        {
            if (m_updateMovement)
            {
                if (m_player.controller.collisionInfo.faceDirection != m_controller.collisionInfo.faceDirection)
                {
                    if (IsPlayerInLineOfSight())
                    {
                        m_updateMovement = false;
                        m_velocity = Vector2.zero;
                        m_animator.PlayWithOnKeyFrameEvent("protect-start", OnProtectStart);
                    }

                }
            }

            if (m_protected)
            {
                if (m_player.controller.collisionInfo.faceDirection == m_controller.collisionInfo.faceDirection)
                {
                    if (m_controller.collisionInfo.faceDirection > 0 && m_player.position.x < position.x)
                    {
                        m_hitBox.gameObject.SetActive(true);
                        m_hurtBox.gameObject.SetActive(true);
                        m_protected = false;

                        m_animator.PlayWithOnKeyFrameEvent("protect-end", OnProtectEnd);
                    }

                    if (m_controller.collisionInfo.faceDirection < 0 && m_player.position.x > position.x)
                    {
                        m_hitBox.gameObject.SetActive(true);
                        m_hurtBox.gameObject.SetActive(true);
                        m_protected = false;

                        m_animator.PlayWithOnKeyFrameEvent("protect-end", OnProtectEnd);
                    }
                }
            }
        }

        public bool IsPlayerInLineOfSight()
        {
            RaycastHit2D hit = Physics2D.Linecast(position + (Vector2.up * 0.5f), m_player.position + (Vector2.up * 0.5f), m_controller.collisionMask);

            return !hit.transform;
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

        private void OnProtectStart(int frame)
        {
            if (!m_animator.currentAnimationName.Equals("protect-start"))
                return;

            if (frame == 5)
            {
                m_hitBox.gameObject.SetActive(false);
                m_hurtBox.gameObject.SetActive(false);
                m_protected = true;
            }
        }

        private void OnProtectEnd(int frame)
        {
            if (!m_animator.currentAnimationName.Equals("protect-end"))
                return;

            if (frame == 6)
            {
                m_updateMovement = true;
                PlayAnimation("move");
            }
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
