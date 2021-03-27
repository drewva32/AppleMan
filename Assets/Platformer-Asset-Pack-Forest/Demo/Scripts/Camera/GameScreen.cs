using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    [RequireComponent(typeof(Camera))]
    public class GameScreen : MonoBehaviour
    {
        public const float DEFAULT_SHAKE_PIXEL_OFFSET = 2f * Constants.PIXEL_SIZE;

        private float m_force;
        private bool m_shaking;
        private float m_val;
        private bool m_trembling;
        private float m_tremblePixels;
        private bool m_trembleDecaying;
        private float m_trembleDecayTime;

        private Vector3 m_originalPosition;
        private Camera m_camera;

        public new Camera camera
        {
            get
            {
                if (m_camera == null)
                {
                    m_camera = GetComponent<Camera>();
                }

                return m_camera;
            }
        }

        private void Start()
        {
            m_force = 0;
            m_shaking = false;
            m_originalPosition = transform.position;
        }

        public virtual void shake(float p_force = 5f)
        {
            m_force = p_force * Constants.PIXEL_SIZE;
            m_shaking = true;
            m_val = Mathf.PI / 2;
        }

        public virtual void tremble(int p_maxPixelDisplacement, float m_timeUntilDecay)
        {
            m_shaking = false;
            transform.position = m_originalPosition;

            m_trembling = true;
            m_tremblePixels = p_maxPixelDisplacement * Constants.PIXEL_SIZE;
            m_trembleDecaying = false;
            m_trembleDecayTime = m_timeUntilDecay;
        }

        public virtual void tremble(int p_maxPixelDisplacement)
        {
            m_shaking = false;
            transform.position = m_originalPosition;

            m_trembling = true;
            m_tremblePixels = p_maxPixelDisplacement * Constants.PIXEL_SIZE;
            m_trembleDecaying = false;
            m_trembleDecayTime = Mathf.Infinity;
        }

        public virtual void stopTremble()
        {
            m_trembleDecaying = true;
        }

        public virtual void Update()
        {
            if (m_shaking)
            {
                transform.position = new Vector3(m_originalPosition.x,
                                                 m_originalPosition.y + (Mathf.Sin(m_val) * m_force),
                                                 m_originalPosition.z);
                m_val += 1.5f;
                m_force -= 0.1f * Constants.PIXEL_SIZE;
                if (m_force <= 0)
                {
                    transform.position = m_originalPosition;
                    m_shaking = false;
                }
            }

            if (m_trembling)
            {
                transform.position = new Vector3(m_originalPosition.x + Random.Range(-m_tremblePixels, m_tremblePixels),
                                                m_originalPosition.y + Random.Range(-m_tremblePixels, m_tremblePixels),
                                                m_originalPosition.z);


                if (m_trembleDecaying)
                {
                    m_tremblePixels -= Time.deltaTime * Constants.PIXEL_SIZE;

                    if (m_tremblePixels <= 0)
                    {
                        transform.position = m_originalPosition;
                        m_trembling = false;
                        m_trembleDecaying = false;
                    }
                }
                else if (m_trembleDecayTime < Mathf.Infinity)
                {
                    m_trembleDecayTime -= Time.deltaTime;

                    if (m_trembleDecayTime <= 0)
                    {
                        m_trembleDecaying = true;
                    }
                }
            }
        }
    }
}
