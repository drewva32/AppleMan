using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RaycastController2D : MonoBehaviour
    {
        protected const int HORIZONTAL_RAY_COUNT = 4;
        protected const int VERTICAL_RAY_COUNT = 4;

        public LayerMask collisionMask;

        protected float m_horizontalRaySpacing;
        protected float m_verticalRaySpacing;

        protected BoxCollider2D m_collider;
        protected RaycastOrigins m_raycastOrigins;

        protected virtual void Start()
        {
            m_collider = GetComponent<BoxCollider2D>();
            CalculateRaySpacing();
        }

        protected void UpdateRaycastOrigins()
        {
            Bounds bounds = m_collider.bounds;
            bounds.Expand(Constants.PIXEL_SIZE * -2);

            m_raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            m_raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            m_raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            m_raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        protected void CalculateRaySpacing()
        {
            Bounds bounds = m_collider.bounds;
            bounds.Expand(Constants.PIXEL_SIZE * -2);

            m_horizontalRaySpacing = bounds.size.y / (HORIZONTAL_RAY_COUNT - 1);
            m_verticalRaySpacing = bounds.size.x / (VERTICAL_RAY_COUNT - 1);
        }

        public RaycastHit2D CheckCustomVerticalCollisions(Vector2 p_velocity, LayerMask p_collisionMask)
        {
            float directionY = Mathf.Sign(p_velocity.y);
            float rayLength = Mathf.Abs(p_velocity.y) + Constants.PIXEL_SIZE;

            Vector2 rayOrigin;
            RaycastHit2D hit = new RaycastHit2D();

            for (int i = 0; i < VERTICAL_RAY_COUNT; i++)
            {
                rayOrigin = directionY < 0 ? m_raycastOrigins.bottomLeft : m_raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (m_verticalRaySpacing * i + p_velocity.x);

                hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, p_collisionMask);

                if (hit)
                {
                    break;
                }
            }

            return hit;
        }

        public struct RaycastOrigins
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }
    }
}