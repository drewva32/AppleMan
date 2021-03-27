using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    public class AnimTime : Singleton<AnimTime>
    {
        private float m_delta;
        private float m_time;
        private bool m_paused;
        private int m_frameCount;
        private float m_timeScale = 1;

        public static float delta
        {
            get { return Instance.m_delta; }
        }

        public static float time
        {
            get { return Instance.m_time; }
        }

        public static bool paused
        {
            get { return Instance.m_paused; }
            set { Instance.m_paused = value; }
        }

        public static float timeScale
        {
            get { return Instance.m_timeScale; }
            set { Instance.m_timeScale = value; }
        }

        public static int frameCount
        {
            get { return Instance.m_frameCount; }
        }

        private void Update()
        {
            if (!paused)
            {
                m_time += Time.deltaTime * timeScale;
                m_delta = Time.deltaTime * timeScale;
                m_frameCount++;
            }
        }
    }
}
