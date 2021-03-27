using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AppleBoy
{
    [RequireComponent(typeof(Controller2D))]
    public class Player2D : Entity2D
    {
        public float moveSpeed = 4;
        public float timeToJumpApex = 0.25f;
        public float maxJumpHeight = 1.25f;
        public float minJumpHeight = 0.75f;

        [Space]
        public Vector2 wallJumpClimb;
        public Vector2 wallJumpOff;
        public Vector2 wallLeap;
        public float wallSlideSpeed = 2;
        public float wallStickTime = 0.25f;

        [Space]
        public float throwPower = 5;

        [Space]
        public Transform liftObjectFramePositions;
        public Transform throwObjectFramePositions;

        [Space]
        public Hurtbox punchHurtBox;

        [Space]
        public LayerMask jumpOnEnemyHeadLayer;
        public float jumpOnEnemyHeadDistanceBelow;
        public float extraJumpMultiplier;
        public float extraJumpButtonLeniency;

        private float m_gravity;
        private float m_maxJumpVelocity;
        private float m_minJumpVelocity;
        private Vector2 m_velocity;

        private int m_wallDirX;
        private bool m_wallSliding;
        private bool m_wallJumping;
        private float m_timeToUnstick;

        float accelerationTimeAirborne = .2f;
        float accelerationTimeGrounded = .1f;
        float velocityXSmoothing;
        float m_targetVelocityX;

        private Vector2 m_directionalInput;

        private bool m_updateInput;
        private bool m_updateAnimations;

        private bool m_jumping;
        private bool m_specialJumping;
        private bool m_jumpButtonPressedWhileInAir;
        private float m_extraJumpButtonLeniencyTimer;

        private bool m_isCarryingObject;
        private Liftable2D m_liftable;

        private Dictionary<int, Vector2> m_liftFramePositions;
        private Dictionary<int, Vector2> m_throwFramePositions;

        public bool updateInput
        {
            get { return m_updateInput; }
            set { m_updateInput = value; }
        }

        public bool updateAnimations
        {
            get { return m_updateAnimations; }
            set { m_updateAnimations = value; }
        }


        public Vector2 velocity
        {
            get { return m_velocity; }
            set { m_velocity = value; }
        }

        public float gravity
        {
            get { return m_gravity; }
        }

        public override void Start()
        {
            base.Start();

            m_updateInput = true;
            m_updateAnimations = true;

            punchHurtBox.gameObject.SetActive(false);

            CalculateLiftFramePositions();
            CalculateJumpVelocity();

            GameHud.Instance.playerHealth.SetHealth(m_health);
        }

        public override void Update()
        {
            base.Update();

            CalculateVelocity();
            HandleWallSliding();
            HandleJumpOnEnemyHead();

            if (!m_updateInput)
            {
                m_velocity.x = 0;
            }

            m_controller.Move(m_velocity * Time.deltaTime, m_directionalInput);

            if (m_controller.collisionInfo.above || m_controller.collisionInfo.below)
            {
                if (m_controller.collisionInfo.slidingDownMaxSlope)
                {
                    m_velocity.y += m_controller.collisionInfo.slopeNormal.y * -m_gravity * Time.deltaTime;
                }
                else
                {
                    m_velocity.y = 0;
                }
            }

            if (m_jumpButtonPressedWhileInAir)
            {
                m_extraJumpButtonLeniencyTimer -= Time.deltaTime;

                if (m_extraJumpButtonLeniencyTimer <= 0)
                {
                    m_jumpButtonPressedWhileInAir = false;
                }
            }

            if (m_updateAnimations)
            {
                UpdateAnimations();
            }
        }

        public void SetLiftableObject(Liftable2D p_liftable)
        {
            if (m_isCarryingObject)
                return;

            m_liftable = p_liftable;
        }

        public void RemoveLiftableObject(Liftable2D p_liftable)
        {
            if (m_isCarryingObject)
                return;

            if (m_liftable == p_liftable)
            {
                m_liftable = null;
            }
        }

        private void CalculateLiftFramePositions()
        {
            m_liftFramePositions = new Dictionary<int, Vector2>();

            for (int i = 0; i < liftObjectFramePositions.childCount; i++)
            {
                string[] frameEncode = liftObjectFramePositions.GetChild(i).name.Split('.');

                if (frameEncode.Length < 2)
                    continue;

                int frameNum = int.Parse(frameEncode[1]);

                m_liftFramePositions.Add(frameNum, liftObjectFramePositions.GetChild(i).localPosition);
            }

            m_throwFramePositions = new Dictionary<int, Vector2>();

            for (int i = 0; i < throwObjectFramePositions.childCount; i++)
            {
                string[] frameEncode = throwObjectFramePositions.GetChild(i).name.Split('.');

                if (frameEncode.Length < 2)
                    continue;

                int frameNum = int.Parse(frameEncode[1]);

                m_throwFramePositions.Add(frameNum, throwObjectFramePositions.GetChild(i).localPosition);
            }

        }

        private void CalculateJumpVelocity()
        {
            m_gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            m_maxJumpVelocity = Mathf.Abs(m_gravity) * timeToJumpApex;
            m_minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(m_gravity) * minJumpHeight);
        }

        public void SetDirectionalInput(Vector2 p_input)
        {
            m_directionalInput = p_input;
        }

        public void OnLiftButtonPressed()
        {
            if (m_liftable)
            {
                if (m_isCarryingObject)
                {
                    CutsceneController.Instance.PlayThrowObjectCutscene(this, m_liftable);
                }
                else
                {
                    m_isCarryingObject = true;
                    CutsceneController.Instance.PlayLiftObjectCutscene(this, m_liftable);
                }
            }
        }

        public void OnAttackButtonPressed()
        {
            m_updateAnimations = false;
            m_updateInput = false;
            m_animator.PlayWithOnKeyFrameEvent("attack", OnAttack);
        }

        public void OnJumpInputDown()
        {
            if (m_wallSliding)
            {
                if (Mathf.Abs(m_wallDirX - m_directionalInput.x) < float.Epsilon)
                {
                    m_velocity.x = -m_wallDirX * wallJumpClimb.x;
                    m_velocity.y = wallJumpClimb.y;
                }
                else if (Mathf.Abs(m_directionalInput.x) < float.Epsilon)
                {
                    m_velocity.x = -m_wallDirX * wallJumpOff.x;
                    m_velocity.y = wallJumpOff.y;
                }
                else
                {
                    m_velocity.x = -m_wallDirX * wallLeap.x;
                    m_velocity.y = wallLeap.y;
                }
            }
            else if (m_controller.collisionInfo.below)
            {
                if (m_controller.collisionInfo.slidingDownMaxSlope)
                {
                    if (Mathf.Abs(m_directionalInput.x - -Mathf.Sign(m_controller.collisionInfo.slopeNormal.x)) > float.Epsilon)
                    {
                        m_velocity.y = m_maxJumpVelocity * m_controller.collisionInfo.slopeNormal.y;
                        m_velocity.x = m_maxJumpVelocity * m_controller.collisionInfo.slopeNormal.x;
                    }
                }
                else
                {
                    m_velocity.y = m_maxJumpVelocity;
                }
            }
            else
            {
                m_jumpButtonPressedWhileInAir = true;
                m_extraJumpButtonLeniencyTimer = extraJumpButtonLeniency;
            }
        }

        public void OnJumpInputUp()
        {
            if (m_specialJumping)
                return;

            if (m_velocity.y > m_minJumpVelocity)
                m_velocity.y = m_minJumpVelocity;
        }

        private void HandleWallSliding()
        {
            if (m_isCarryingObject)
                return;

            m_wallDirX = m_controller.collisionInfo.left ? -1 : 1;
            m_wallSliding = false;

            if (m_wallDirX < 0)
            {
                if (!string.IsNullOrEmpty(m_controller.collisionInfo.leftTag) && m_controller.collisionInfo.leftTag.Equals(m_controller.jumpThroughTag))
                {
                    return;
                }
            }

            if (m_wallDirX > 0)
            {
                if (!string.IsNullOrEmpty(m_controller.collisionInfo.rightTag) && m_controller.collisionInfo.rightTag.Equals(m_controller.jumpThroughTag))
                {
                    return;
                }
            }

            if ((m_controller.collisionInfo.left || m_controller.collisionInfo.right) && !m_controller.collisionInfo.below && m_velocity.y < 0)
            {
                m_wallSliding = true;

                if (m_velocity.y < -wallSlideSpeed)
                {
                    m_velocity.y = -wallSlideSpeed;
                }

                if (m_timeToUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    m_velocity.x = 0;

                    if (Mathf.Abs(m_directionalInput.x - m_wallDirX) > float.Epsilon && Mathf.Abs(m_directionalInput.x) > float.Epsilon)
                    {
                        m_timeToUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        m_timeToUnstick = wallStickTime;
                    }
                }
                else
                {
                    m_timeToUnstick = wallStickTime;
                }
            }
        }

        private void HandleJumpOnEnemyHead()
        {
            if (!m_controller.collisionInfo.below && m_velocity.y < 0)
            {
                Vector2 step = new Vector2(0f, -jumpOnEnemyHeadDistanceBelow);
                RaycastHit2D enemyHit = m_controller.CheckCustomVerticalCollisions(step, jumpOnEnemyHeadLayer);

                if (enemyHit.collider)
                {
                    Enemy2D enemy = enemyHit.collider.GetComponent<Enemy2D>();

                    if (enemy && enemy.active)
                    {
                        if (enemy.canJumpOnHead)
                        {
                            enemy.OnDeath(new DamageInfo());
                        }

                        if (m_jumpButtonPressedWhileInAir)
                        {
                            m_specialJumping = true;
                            m_velocity.y = m_maxJumpVelocity * extraJumpMultiplier;
                        }
                        else
                        {
                            m_velocity.y = m_maxJumpVelocity;
                        }

                    }
                }
            }
        }

        private void CalculateVelocity()
        {
            m_targetVelocityX = m_directionalInput.x * moveSpeed;
            m_velocity.x = Mathf.SmoothDamp(m_velocity.x, m_targetVelocityX, ref velocityXSmoothing, (m_controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            m_velocity.y += m_gravity * Time.deltaTime;
        }

        private void UpdateAnimations()
        {
            scaleX = m_controller.collisionInfo.faceDirection;

            if (!m_controller.collisionInfo.below)
            {
                if (m_wallSliding)
                {
                    PlayAnimation("wall-slide");
                }
                else if (m_velocity.y > float.Epsilon)
                {
                    m_jumping = true;

                    if (m_isCarryingObject)
                    {
                        PlayAnimation("jump-with-item");
                    }
                    else if (m_specialJumping)
                    {
                        PlayAnimation("double-jump");
                    }
                    else
                    {
                        PlayAnimation("jump");
                    }
                }
                else if (m_velocity.y < float.Epsilon)
                {
                    if (m_specialJumping && m_animator.currentAnimationName.Equals("double-jump") && m_animator.CurrentFrame < 4)
                    {
                        return;
                    }

                    if (m_isCarryingObject)
                    {
                        PlayAnimation("fall-with-item");
                    }
                    else
                    {
                        PlayAnimation("fall");
                    }

                    if (m_specialJumping)
                    {
                        m_specialJumping = false;
                    }
                }
                else if (!m_specialJumping)
                {
                    if (m_isCarryingObject)
                    {
                        PlayAnimation("jump-apex-with-item");
                    }
                    else
                    {
                        PlayAnimation("jump-apex");
                    }
                }
            }
            else if (m_controller.collisionInfo.below)
            {
                if (m_specialJumping)
                {
                    m_specialJumping = false;
                }

                if (m_jumping)
                {
                    m_jumping = false;
                    m_updateAnimations = false;

                    if (m_isCarryingObject)
                    {
                        PlayAnimation("land-with-item", OnLand);
                    }
                    else
                    {
                        PlayAnimation("land", OnLand);
                    }
                }
                else if (Mathf.Abs(m_directionalInput.x) > float.Epsilon)
                {
                    if (m_isCarryingObject)
                    {
                        PlayAnimation("run-with-item");
                    }
                    else
                    {
                        PlayAnimation("run");
                    }

                }
                else
                {
                    if (m_isCarryingObject)
                    {
                        PlayAnimation("idle-with-item");
                    }
                    else
                    {
                        PlayAnimation("idle");
                    }
                }
            }
        }


        private void OnLand()
        {
            m_updateAnimations = true;
        }

        public void OnLiftObject(int frame)
        {
            if (m_liftFramePositions.ContainsKey(frame))
            {
                m_liftable.localPosition = m_liftFramePositions[frame];
            }
        }

        public void OnThrowObject(int frame)
        {
            if (m_throwFramePositions.ContainsKey(frame))
            {
                m_liftable.localPosition = m_throwFramePositions[frame];
            }

            if (frame == 5)
            {
                m_isCarryingObject = false;

                if (m_liftable)
                {
                    m_liftable.transform.SetParent(null);
                    Vector2 throwForce = new Vector2(m_controller.collisionInfo.faceDirection * throwPower, throwPower);
                    m_liftable.Throw(throwForce, m_gravity);
                    m_liftable = null;
                }
            }
        }

        public void OnAttack(int frame)
        {
            if (frame == 2)
            {
                punchHurtBox.gameObject.SetActive(true);
            }

            if (frame == 4)
            {
                punchHurtBox.gameObject.SetActive(false);
            }

            if (frame == 7)
            {
                m_updateInput = true;
                m_updateAnimations = true;
            }
        }

        public override void OnDamaged(DamageInfo p_damageInfo)
        {
            base.OnDamaged(p_damageInfo);

            m_updateAnimations = false;

            GameHud.Instance.playerHealth.RefreshHealthUI();
        }

        protected override void OnDamageComplete()
        {
            base.OnDamageComplete();

            m_updateAnimations = true;
        }

        public override void OnDeath(DamageInfo p_damageInfo)
        {
            base.OnDeath(p_damageInfo);

            m_updateAnimations = false;
            m_updateInput = false;

            GameHud.Instance.playerHealth.RefreshHealthUI();
        }

        protected override void OnDeathComplete()
        {

        }

        protected override void OnHeal(HealInfo p_healInfo)
        {
            base.OnHeal(p_healInfo);

            GameHud.Instance.playerHealth.RefreshHealthUI();
        }
    }
}
