using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AppleBoy
{
    public struct DamageInfo
    {
        public int value;
    }

    public struct HealInfo
    {
        public int value;
    }

    public enum Team
    {
        PLAYER,
        ENEMY,
    }

    public class Health : MonoBehaviour
    {
        public int maxValue;
        public bool flickerWhenInvincibile;
        public float invincibilityTime;


        [HideInInspector]
        public int currentValue;

        public Action<HealInfo> Healed = delegate { };
        public Action<DamageInfo> Damaged = delegate { };
        public Action<DamageInfo> Death = delegate { };

        private Hitbox m_hitBox;
        private SpriteRenderer m_renderer;
        private float m_invincibilityTimer;

        private void Start()
        {
            currentValue = maxValue;

            m_hitBox = GetComponentInChildren<Hitbox>();
            m_hitBox.SetHealth(this);

            m_renderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            if (m_invincibilityTimer > 0)
            {
                if (flickerWhenInvincibile)
                {
                    m_renderer.enabled = Time.frameCount % 4 == 0;
                }

                m_invincibilityTimer -= Time.deltaTime;

                if (m_invincibilityTimer <= 0)
                {
                    if (flickerWhenInvincibile)
                    {
                        m_renderer.enabled = true;
                    }

                    m_hitBox.gameObject.SetActive(true);
                    m_invincibilityTimer = 0;
                }
            }
        }

        public void Heal(HealInfo p_healInfo)
        {
            currentValue += p_healInfo.value;

            if (currentValue > maxValue)
            {
                currentValue = maxValue;
            }

            Healed(p_healInfo);
        }

        public void Damage(DamageInfo p_damageInfo)
        {
            if (m_invincibilityTimer > 0)
                return;

            currentValue -= p_damageInfo.value;

            if (currentValue <= 0)
            {
                currentValue = 0;
                Death(p_damageInfo);
            }
            else
            {
                m_invincibilityTimer = invincibilityTime;

                if (m_invincibilityTimer > 0)
                {
                    m_hitBox.gameObject.SetActive(false);
                }

                Damaged(p_damageInfo);
            }
        }
    }
}
