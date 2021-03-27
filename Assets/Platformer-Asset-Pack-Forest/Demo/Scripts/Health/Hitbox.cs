using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.AccessControl;

namespace AppleBoy
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class Hitbox : MonoBehaviour
    {
        public Team team;
        private Health m_health;

        public void SetHealth(Health p_health)
        {
            m_health = p_health;
        }

        public void Damage(DamageInfo p_damageInfo)
        {
            if (m_health)
            {
                m_health.Damage(p_damageInfo);
            }
        }
    }
}
