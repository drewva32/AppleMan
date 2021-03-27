using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppleBoy
{
    public class ParallaxBackground : MonoBehaviour
    {
        public float unitsUntilLoop;

        private Vector2 m_startPosition;
        private Material m_material;

        private void Start()
        {
            m_startPosition = Camera.main.transform.position;
            m_material = GetComponent<Image>().material;
        }

        private void Update()
        {
            float distFromStart = Mathf.Abs(m_startPosition.x - Camera.main.transform.position.x);
            float offsetAmount = (distFromStart % unitsUntilLoop) / unitsUntilLoop;

            m_material.mainTextureOffset = new Vector2(offsetAmount, 0);
        }

    }
}
