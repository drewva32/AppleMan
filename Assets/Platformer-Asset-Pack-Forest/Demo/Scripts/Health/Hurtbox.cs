using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class Hurtbox : MonoBehaviour
    {
        public Team team;
        public int damageAmount;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            Hitbox hitBox = collision.GetComponent<Hitbox>();

            if (hitBox != null && hitBox.team != team)
            {
                hitBox.Damage(new DamageInfo() { value = damageAmount });
            }
        }
    }
}
