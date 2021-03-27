using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Liftable2D : Entity2D
    {
        public string liftable;
        public Transform pickupPivotRight;
        public Transform pickupPivotLeft;

        public int contentCount;
        public List<Pickup2D> randomContents;

        private bool m_updateThrow;
        private Vector2 m_throwVelocity;
        private float m_gravity;

        private Hurtbox m_hurtBox;

        public Vector2 localPosition
        {
            get { return transform.localPosition; }
            set { transform.localPosition = value; }
        }

        public override void Start()
        {
            base.Start();

            PlayAnimation("idle");

            m_hurtBox = GetComponentInChildren<Hurtbox>();

            if (m_hurtBox)
            {
                m_hurtBox.gameObject.SetActive(false);
            }
        }

        public override void Update()
        {
            if (m_updateThrow)
            {
                if (m_controller.collisionInfo.Any())
                {
                    OnLand();
                    return;
                }

                m_throwVelocity.y += m_gravity * Time.deltaTime;
                m_controller.Move(m_throwVelocity * Time.deltaTime);
            }
        }

        public void Throw(Vector2 p_velocity, float p_gravity)
        {
            m_updateThrow = true;
            m_throwVelocity = p_velocity;
            m_gravity = p_gravity;

            if (m_hurtBox)
            {
                m_hurtBox.gameObject.SetActive(true);
            }

        }

        protected virtual void OnLand()
        {
            m_updateThrow = false;

            if (m_hurtBox)
            {
                m_hurtBox.gameObject.SetActive(false);
            }

            OnDeath(new DamageInfo());
        }

        public override void OnDeath(DamageInfo p_damageInfo)
        {
            base.OnDeath(p_damageInfo);

            float angle;
            Pickup2D pickupPrefab;
            Pickup2D pickup;
            Vector2 forceDir;
            float forcePow = 6;

            for (int i = 0; i < contentCount; i++)
            {
                angle = (45 * i) - 45;
                pickupPrefab = randomContents[Random.Range(0, randomContents.Count)];
                pickup = Instantiate(pickupPrefab);
                pickup.position = GetCirclePointXY(position, 0.5f, angle);

                forceDir = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
                pickup.AddForce(forceDir * forcePow, m_player.gravity);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Player"))
            {
                Player2D player = collision.GetComponent<Player2D>();
                player.SetLiftableObject(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.Equals("Player"))
            {
                Player2D player = collision.GetComponent<Player2D>();
                player.RemoveLiftableObject(this);
            }
        }
    }
}
