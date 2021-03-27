using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppleBoy
{
    public class UISpriteCounter : MonoBehaviour
    {
        public List<Sprite> numberImages;

        private Dictionary<char, Sprite> m_numberMap;

        public void Start()
        {
            m_numberMap = new Dictionary<char, Sprite>();

            for (int i = 0; i < 10; i++)
            {
                m_numberMap.Add(char.Parse(i.ToString()), numberImages[i]);
            }
        }

        public void SetCount(int p_count)
        {
            for (int i = transform.childCount - 1; i > -1; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            string numberText = p_count.ToString("D2");

            for (int i = 0; i < numberText.Length; i++)
            {
                char num = numberText[i];
                GameObject numObj = new GameObject("Number", typeof(Image));
                numObj.transform.SetParent(transform, false);
                Image numImage = numObj.GetComponent<Image>();
                numImage.sprite = m_numberMap[num];
            }
        }
    }
}
