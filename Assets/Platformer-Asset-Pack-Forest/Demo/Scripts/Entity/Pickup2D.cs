using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppleBoy;

namespace AppleBoy
{
    public class Pickup2D : Entity2D
    {
        public string item;
        public Transform pickupEffect;

        private bool m_updateForce;
        private Vector2 m_velocity;
        private float m_gravity;

        public override void Start()
        {
            base.Start();

            PlayAnimation("idle");
        }

        public override void Update()
        {
            if (m_updateForce)
            {
                if (m_controller.collisionInfo.Any())
                {
                    if (m_controller.collisionInfo.above || m_controller.collisionInfo.below)
                    {
                        m_velocity = new Vector2(m_velocity.x, -m_velocity.y) * 0.5f;
                    }
                    else if (m_controller.collisionInfo.left || m_controller.collisionInfo.right)
                    {
                        m_velocity = new Vector2(-m_velocity.x, m_velocity.y) * 0.5f;
                    }
                }

                m_velocity.y += m_gravity * Time.deltaTime;
                m_controller.Move(m_velocity * Time.deltaTime);
            }
        }

        public void AddForce(Vector2 p_velocity, float p_gravity)
        {
            m_updateForce = true;
            m_velocity = p_velocity;
            m_gravity = p_gravity;
        }

        public override void OnDeath(DamageInfo p_damageInfo)
        {
            Transform effect = Instantiate(pickupEffect);
            effect.position = position;

            SpriteRenderer effectRenderer = effect.GetComponentInChildren<SpriteRenderer>();
            effectRenderer.sortingOrder = m_player.sortingOrder + 1;

            OnDeathComplete();
        }

        private void GivePickupToPlayer()
        {
            switch (item)
            {
                case "Coin":
                    GameHud.Instance.AddCoins(1);
                    break;

                case "Extra Life":
                    GameHud.Instance.AddLives(1);
                    break;

                case "Health":
                    m_player.health.Heal(new HealInfo() { value = 1 });
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Player"))
            {
                m_updateForce = false;

                GivePickupToPlayer();

                OnDeath(new DamageInfo());
            }
        }
    }
}
