using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    public class UIHealth : MonoBehaviour
    {
        public RectTransform healthLayout;
        public RectTransform healthEndPieceLeftPrefab;
        public RectTransform healthMiddlePiecePrefab;
        public RectTransform healthEndPieceRightPrefab;

        private Health m_health;

        public void SetHealth(Health p_health)
        {
            m_health = p_health;

            RefreshHealthUI();
        }

        public void RefreshHealthUI()
        {
            for (int i = transform.childCount - 1; i > -1; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < m_health.currentValue; i++)
            {
                RectTransform prefabPiece;

                if (i == 0)
                {
                    prefabPiece = healthEndPieceLeftPrefab;
                }
                else if (i == m_health.maxValue - 1)
                {
                    prefabPiece = healthEndPieceRightPrefab;
                }
                else
                {
                    prefabPiece = healthMiddlePiecePrefab;
                }

                RectTransform healthPiece = Instantiate(prefabPiece);
                healthPiece.SetParent(transform, false);
            }
        }

    }
}
