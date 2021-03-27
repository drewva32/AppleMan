using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Controller2D : RaycastController2D
    {
        private const float MAX_SLOPE_ANGLE = 60;

        public CollisionInfo collisionInfo;
        public string jumpThroughTag = "Respawn";

        private Vector2 m_playerInput;

        protected override void Start()
        {
            base.Start();

            collisionInfo.faceDirection = 1;
        }

        public void Move(Vector2 p_velocity, bool p_standingOnPlatform = false)
        {
            Move(p_velocity, m_playerInput, p_standingOnPlatform);
        }

        public void Move(Vector2 p_velocity, Vector2 p_input, bool p_standingOnPlatform = false)
        {
            UpdateRaycastOrigins();

            collisionInfo.Reset();
            collisionInfo.velocityOld = p_velocity;

            m_playerInput = p_input;

            if (p_velocity.y < 0)
            {
                DescendSlope(ref p_velocity);
            }

            if (Mathf.Abs(p_velocity.x) > float.Epsilon)
            {
                collisionInfo.faceDirection = (int)Mathf.Sign(p_velocity.x);
            }

            HorizontalCollisions(ref p_velocity);

            if (Mathf.Abs(p_velocity.y) > float.Epsilon)
            {
                VerticalCollisions(ref p_velocity);
            }

            transform.Translate(p_velocity);

            if (p_standingOnPlatform)
            {
                collisionInfo.below = true;
            }
        }



        private void VerticalCollisions(ref Vector2 p_velocity)
        {
            float directionY = Mathf.Sign(p_velocity.y);
            float rayLength = Mathf.Abs(p_velocity.y) + Constants.PIXEL_SIZE;

            Vector2 rayOrigin;
            RaycastHit2D hit;

            for (int i = 0; i < VERTICAL_RAY_COUNT; i++)
            {
                rayOrigin = directionY < 0 ? m_raycastOrigins.bottomLeft : m_raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (m_verticalRaySpacing * i + p_velocity.x);

                hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * rayLength * directionY, Color.red);

                if (hit)
                {
                    if (hit.collider.tag.Equals(jumpThroughTag))
                    {
                        if (directionY > 0 || hit.distance <= 0)
                        {
                            continue;
                        }

                        if (collisionInfo.fallingThroughPlatform)
                        {
                            continue;
                        }

                        if (m_playerInput.y < 0)
                        {
                            collisionInfo.fallingThroughPlatform = true;
                            Invoke("ResetFallingThroughPlatform", 0.5f);
                            continue;
                        }
                    }

                    p_velocity.y = (hit.distance - Constants.PIXEL_SIZE) * directionY;
                    rayLength = hit.distance;

                    if (collisionInfo.climbingSlope)
                    {
                        p_velocity.x = p_velocity.y / Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(p_velocity.x);
                    }

                    collisionInfo.below = directionY < 0;
                    collisionInfo.above = directionY > 0;

                    if (collisionInfo.below)
                    {
                        collisionInfo.belowTag = hit.collider.tag;
                    }

                    if (collisionInfo.above)
                    {
                        collisionInfo.aboveTag = hit.collider.tag;
                    }
                }
            }

            if (collisionInfo.climbingSlope)
            {
                float directionX = Mathf.Sign(p_velocity.x);
                rayLength = Mathf.Abs(p_velocity.x) + Constants.PIXEL_SIZE;
                rayOrigin = (directionX < 0 ? m_raycastOrigins.bottomLeft : m_raycastOrigins.bottomRight) + Vector2.up * p_velocity.y;
                hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                    if (Mathf.Abs(slopeAngle - collisionInfo.slopeAngle) > float.Epsilon)
                    {
                        p_velocity.x = (hit.distance - Constants.PIXEL_SIZE) * directionX;
                        collisionInfo.slopeAngle = slopeAngle;
                        collisionInfo.slopeNormal = hit.normal;
                    }
                }
            }
        }

        private void HorizontalCollisions(ref Vector2 p_velocity)
        {
            float directionX = collisionInfo.faceDirection;
            float rayLength = Mathf.Abs(p_velocity.x) + Constants.PIXEL_SIZE;

            if (Mathf.Abs(p_velocity.x) < Constants.PIXEL_SIZE)
            {
                rayLength = Constants.PIXEL_SIZE * 2;
            }

            Vector2 rayOrigin;
            RaycastHit2D hit;

            for (int i = 0; i < HORIZONTAL_RAY_COUNT; i++)
            {
                rayOrigin = directionX < 0 ? m_raycastOrigins.bottomLeft : m_raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (m_horizontalRaySpacing * i);

                hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * rayLength * directionX, Color.red);

                if (hit)
                {
                    if (hit.distance <= 0)
                        continue;

                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                    if (i == 0 && slopeAngle <= MAX_SLOPE_ANGLE)
                    {
                        if (collisionInfo.descendingSlope)
                        {
                            collisionInfo.descendingSlope = false;
                            p_velocity = collisionInfo.velocityOld;
                        }
                        float distanceToSlopeStart = 0;

                        if (Mathf.Abs(slopeAngle - collisionInfo.slopeAngleOld) > float.Epsilon)
                        {
                            distanceToSlopeStart = hit.distance - Constants.PIXEL_SIZE;
                            p_velocity.x -= distanceToSlopeStart * directionX;
                        }

                        ClimbSlope(ref p_velocity, slopeAngle, hit.normal);

                        p_velocity.x += distanceToSlopeStart * directionX;
                    }

                    if (!collisionInfo.climbingSlope || slopeAngle > MAX_SLOPE_ANGLE)
                    {
                        p_velocity.x = (hit.distance - Constants.PIXEL_SIZE) * directionX;
                        rayLength = hit.distance;

                        if (collisionInfo.climbingSlope)
                        {
                            p_velocity.y = Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(p_velocity.x);
                        }

                        collisionInfo.left = directionX < 0;
                        collisionInfo.right = directionX > 0;

                        if (collisionInfo.left)
                        {
                            collisionInfo.leftTag = hit.collider.tag;
                        }

                        if (collisionInfo.right)
                        {
                            collisionInfo.rightTag = hit.collider.tag;
                        }
                    }
                }
            }
        }

        private void ClimbSlope(ref Vector2 p_velocity, float p_slopeAngle, Vector2 p_slopeNormal)
        {
            float moveDistance = Mathf.Abs(p_velocity.x);
            float climbVelocityY = Mathf.Sin(p_slopeAngle * Mathf.Deg2Rad) * moveDistance;

            if (p_velocity.y <= climbVelocityY)
            {
                p_velocity.y = climbVelocityY;
                p_velocity.x = Mathf.Cos(p_slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(p_velocity.x);
                collisionInfo.below = true;
                collisionInfo.climbingSlope = true;
                collisionInfo.slopeAngle = p_slopeAngle;
                collisionInfo.slopeNormal = p_slopeNormal;
            }
        }

        private void DescendSlope(ref Vector2 p_velocity)
        {
            RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(m_raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(p_velocity.y) + Constants.PIXEL_SIZE, collisionMask);
            RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(m_raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(p_velocity.y) + Constants.PIXEL_SIZE, collisionMask);

            if (maxSlopeHitLeft ^ maxSlopeHitRight)
            {
                SlideDownMaxSlope(maxSlopeHitLeft, ref p_velocity);
                SlideDownMaxSlope(maxSlopeHitRight, ref p_velocity);
            }

            if (collisionInfo.slidingDownMaxSlope)
            {
                return;
            }

            float directionX = Mathf.Sign(p_velocity.x);
            Vector2 rayOrigin = directionX < 0 ? m_raycastOrigins.bottomRight : m_raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle > 0 && slopeAngle <= MAX_SLOPE_ANGLE)
                {
                    if (Mathf.Abs(Mathf.Sign(hit.normal.x) - directionX) < float.Epsilon)
                    {
                        if (hit.distance - Constants.PIXEL_SIZE <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(p_velocity.x))
                        {
                            float moveDistance = Mathf.Abs(p_velocity.x);
                            float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

                            p_velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(p_velocity.x);
                            p_velocity.y -= descendVelocityY;

                            collisionInfo.slopeAngle = slopeAngle;
                            collisionInfo.descendingSlope = true;
                            collisionInfo.below = true;
                            collisionInfo.slopeNormal = hit.normal;
                        }
                    }
                }
            }
        }

        private void SlideDownMaxSlope(RaycastHit2D p_hit, ref Vector2 p_velocity)
        {
            if (p_hit)
            {
                float slopeAngle = Vector2.Angle(p_hit.normal, Vector2.up);

                if (slopeAngle > MAX_SLOPE_ANGLE)
                {
                    p_velocity.x = Mathf.Sign(p_hit.normal.x) * ((Mathf.Abs(p_velocity.y) - p_hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad));
                    collisionInfo.slopeAngle = slopeAngle;
                    collisionInfo.slidingDownMaxSlope = true;
                    collisionInfo.slopeNormal = p_hit.normal;
                }
            }
        }

        private void ResetFallingThroughPlatform()
        {
            collisionInfo.fallingThroughPlatform = false;
        }

        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;

            public string aboveTag, belowTag;
            public string leftTag, rightTag;

            public bool climbingSlope, descendingSlope, slidingDownMaxSlope;
            public float slopeAngle, slopeAngleOld;
            public Vector2 slopeNormal;

            public Vector2 velocityOld;

            public int faceDirection;

            public bool fallingThroughPlatform;
            public int fallingThroughPlatformId;

            public bool Any()
            {
                return above || below || right || left;
            }

            public void Reset()
            {
                above = below = false;
                left = right = false;

                aboveTag = belowTag = string.Empty;
                leftTag = rightTag = string.Empty;

                climbingSlope = descendingSlope = slidingDownMaxSlope = false;
                slopeAngleOld = slopeAngle;
                slopeAngle = 0;
                slopeNormal = Vector2.zero;
            }
        }
    }
}
