using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    [RequireComponent(typeof(Controller2D))]
    public class Entity2D : MonoBehaviour
    {
        protected bool m_active;
        protected Controller2D m_controller;
        protected SpriteRenderer m_renderer;
        protected SpriteAnimation m_animator;
        protected Health m_health;
        protected Player2D m_player;

        public bool active
        {
            get { return m_active; }
        }

        public Vector2 position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public new SpriteRenderer renderer
        {
            get { return m_renderer; }
        }

        public SpriteAnimation animator
        {
            get { return m_animator; }
        }

        public Health health
        {
            get { return m_health; }
        }

        public Controller2D controller
        {
            get { return m_controller; }
        }

        public int sortingOrder
        {
            get { return renderer.sortingOrder; }
            set { renderer.sortingOrder = value; }
        }

        public float scaleX
        {
            get { return transform.localScale.x; }
            set { transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z); }
        }

        public virtual void Start()
        {
            m_active = true;

            m_controller = GetComponent<Controller2D>();
            m_renderer = GetComponentInChildren<SpriteRenderer>();
            m_animator = GetComponentInChildren<SpriteAnimation>();

            m_health = GetComponent<Health>();

            if (m_health)
            {
                m_health.Damaged += OnDamaged;
                m_health.Death += OnDeath;
                m_health.Healed += OnHeal;
            }

            m_player = FindObjectOfType<Player2D>();
        }

        public virtual void Update()
        {

        }


        protected virtual void PlayAnimation(string p_animation, Action p_completeAction = null)
        {
            if (!m_animator.currentAnimationName.Equals(p_animation))
            {
                if (p_completeAction != null)
                {
                    m_animator.PlayWithCallBack(p_animation, p_completeAction);
                }
                else if (!m_animator.Play(p_animation))
                {
                    Debug.LogWarningFormat("Could not play animation {0} please check sprite animation component.", p_animation);
                }
            }
        }

        public virtual void OnDamaged(DamageInfo p_damageInfo)
        {
            m_animator.PlayWithCallBack("damaged", OnDamageComplete);
        }

        public virtual void OnDeath(DamageInfo p_damageInfo)
        {
            m_active = false;
            m_animator.PlayWithCallBack("death", OnDeathComplete);
        }

        protected virtual void OnDamageComplete()
        {
            m_animator.Play("idle");
        }

        protected virtual void OnDeathComplete()
        {
            Destroy(gameObject);
        }

        protected virtual void OnHeal(HealInfo p_healInfo)
        {

        }

        public static Vector2 GetCirclePointXY(Vector2 center, float radius, float angle)
        {
            Vector2 pos;
            pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            pos.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);

            return pos;
        }

        public static void orbitMovementXY(List<Transform> orbs, Vector2 center, float radius, float speed, float time)
        {
            for (int i = 0; i < orbs.Count; i++)
            {
                float a = (360 / orbs.Count) * i;

                if (orbs[i] != null)
                {
                    Transform orb = orbs[i];
                    Vector3 pos = GetCirclePointXY(center, radius, Mathf.RoundToInt(a + (time * speed)));
                    orb.position = pos;
                }
            }
        }

    }
}
