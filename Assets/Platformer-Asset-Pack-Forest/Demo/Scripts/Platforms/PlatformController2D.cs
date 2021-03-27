using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    public class PlatformController2D : RaycastController2D
    {
        public LayerMask passengerMask;
        public float m_moveSpeed;
        public bool m_loops;

        public Vector2[] localWaypoints;

        private int m_fromWaypointIndex;
        private float m_percentBetweenWaypoints;
        private Vector2[] globalWaypoints;

        private Vector2 m_velocity;
        private HashSet<Transform> m_passengers;

        private List<PassengerMovement> m_passengerMovement;
        private Dictionary<Transform, Controller2D> m_passengerDictionary;

        protected override void Start()
        {
            base.Start();

            m_passengers = new HashSet<Transform>();
            m_passengerMovement = new List<PassengerMovement>();
            m_passengerDictionary = new Dictionary<Transform, Controller2D>();

            globalWaypoints = new Vector2[localWaypoints.Length];

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                globalWaypoints[i] = localWaypoints[i] + (Vector2)transform.position;
            }
        }

        private void Update()
        {
            UpdateRaycastOrigins();

            m_velocity = CalculatePlatformMovement();

            CalculatePassengerMovement(m_velocity);

            MovePassengers(true);
            transform.Translate(m_velocity);
            MovePassengers(false);
        }

        private Vector2 CalculatePlatformMovement()
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

                if (!m_loops)
                {
                    if (m_fromWaypointIndex >= globalWaypoints.Length - 1)
                    {
                        m_fromWaypointIndex = 0;
                        System.Array.Reverse(globalWaypoints);
                    }
                }
            }

            return newPos - (Vector2)transform.position;
        }

        private void MovePassengers(bool p_beforeMovePlatform)
        {
            for (int i = 0; i < m_passengerMovement.Count; i++)
            {
                var passenger = m_passengerMovement[i];

                if (!m_passengerDictionary.ContainsKey(passenger.transform))
                {
                    m_passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
                }

                if (passenger.moveBeforePlatform == p_beforeMovePlatform && m_passengerDictionary[passenger.transform])
                {
                    m_passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
                }
            }
        }

        private void CalculatePassengerMovement(Vector2 p_velocity)
        {
            m_passengers = new HashSet<Transform>();
            m_passengerMovement = new List<PassengerMovement>();

            float directionX = Mathf.Sign(p_velocity.x);
            float directionY = Mathf.Sign(p_velocity.y);

            if (Mathf.Abs(p_velocity.y) > float.Epsilon)
            {
                float rayLength = Mathf.Abs(p_velocity.y) + Constants.PIXEL_SIZE;

                Vector2 rayOrigin;
                RaycastHit2D hit;

                for (int i = 0; i < VERTICAL_RAY_COUNT; i++)
                {
                    rayOrigin = directionY < 0 ? m_raycastOrigins.bottomLeft : m_raycastOrigins.topLeft;
                    rayOrigin += Vector2.right * (m_verticalRaySpacing * i);

                    hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                    if (hit && hit.distance > 0)
                    {
                        if (!m_passengers.Contains(hit.transform))
                        {
                            float pushX = (directionY > 0) ? p_velocity.x : 0;
                            float pushY = p_velocity.y - (hit.distance - Constants.PIXEL_SIZE) * directionY;

                            m_passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), directionY > 0, true));

                            m_passengers.Add(hit.transform);
                        }
                    }
                }
            }

            if (Mathf.Abs(p_velocity.x) > float.Epsilon)
            {
                float rayLength = Mathf.Abs(p_velocity.x) + Constants.PIXEL_SIZE;

                Vector2 rayOrigin;
                RaycastHit2D hit;

                for (int i = 0; i < HORIZONTAL_RAY_COUNT; i++)
                {
                    rayOrigin = directionX < 0 ? m_raycastOrigins.bottomLeft : m_raycastOrigins.bottomRight;
                    rayOrigin += Vector2.up * (m_horizontalRaySpacing * i);

                    hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                    if (hit && hit.distance > 0)
                    {
                        if (!m_passengers.Contains(hit.transform))
                        {
                            float pushX = p_velocity.x - (hit.distance - Constants.PIXEL_SIZE) * directionX;
                            float pushY = -Constants.PIXEL_SIZE;

                            m_passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), false, true));

                            m_passengers.Add(hit.transform);
                        }
                    }
                }
            }

            if (directionY < 0 || (Mathf.Abs(p_velocity.y) < float.Epsilon && Mathf.Abs(p_velocity.x) > float.Epsilon))
            {
                float rayLength = Constants.PIXEL_SIZE * 2;

                Vector2 rayOrigin;
                RaycastHit2D hit;

                for (int i = 0; i < VERTICAL_RAY_COUNT; i++)
                {
                    rayOrigin = m_raycastOrigins.topLeft + Vector2.right * (m_verticalRaySpacing * i);

                    hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                    if (hit && hit.distance > 0)
                    {
                        if (!m_passengers.Contains(hit.transform))
                        {
                            float pushX = p_velocity.x;
                            float pushY = p_velocity.y;

                            m_passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), true, false));

                            m_passengers.Add(hit.transform);
                        }
                    }
                }
            }
        }

        public struct PassengerMovement
        {
            public Transform transform;
            public Vector2 velocity;
            public bool standingOnPlatform;
            public bool moveBeforePlatform;

            public PassengerMovement(Transform p_transform, Vector2 p_velocity, bool p_standingOnPlatform, bool p_moveBeforePlatform)
            {
                transform = p_transform;
                velocity = p_velocity;
                standingOnPlatform = p_standingOnPlatform;
                moveBeforePlatform = p_moveBeforePlatform;
            }
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