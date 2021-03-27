using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace AppleBoy
{
    public class PauseMenu : Singleton<PauseMenu>
    {
        public const float INPUT_BUFFER = 0.12F;
        public const float INPUT_THRESHOLD = 0.25F;
        public const float LOADING_WAIT_TIME = 0.5F;

        public CanvasGroup canvas;
        public RectTransform optionLayout;

        public CanvasGroup loadingCover;

        private int m_stretchFill;
        private int m_pixelPerfect;

        private int m_currentlySelectedOption;

        private float m_directionalInputTimer;
        private float m_selectedInputTimer;

        public IEnumerator Start()
        {
            canvas.alpha = 1;
            loadingCover.alpha = 1;


            m_stretchFill = PlayerPrefs.GetInt("stretchFill", 0);
            m_pixelPerfect = PlayerPrefs.GetInt("pixelPerfect", 0);

            yield return new WaitForSeconds(LOADING_WAIT_TIME);

            

            canvas.alpha = 0;
            loadingCover.alpha = 0;

            UnPause();
        }

        public void Update()
        {
            if (canvas.alpha <= 0)
                return;

            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            bool selectPressed = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X);

            if (m_directionalInputTimer > 0)
            {
                m_directionalInputTimer -= Time.unscaledDeltaTime;
            }

            if (m_selectedInputTimer > 0)
            {
                m_selectedInputTimer -= Time.unscaledDeltaTime;
            }

            if (m_directionalInputTimer <= 0)
            {
                if (directionalInput.y < -INPUT_THRESHOLD)
                {
                    m_currentlySelectedOption++;
                    m_directionalInputTimer = INPUT_BUFFER;
                }

                if (directionalInput.y > INPUT_THRESHOLD)
                {
                    m_currentlySelectedOption--;
                    m_directionalInputTimer = INPUT_BUFFER;
                }
            }

            if (m_currentlySelectedOption >= optionLayout.childCount)
            {
                m_currentlySelectedOption = 0;
            }

            if (m_currentlySelectedOption < 0)
            {
                m_currentlySelectedOption = optionLayout.childCount - 1;
            }

            for (int i = 0; i < optionLayout.childCount; i++)
            {
                optionLayout.GetChild(i).GetComponent<CanvasGroup>().alpha = m_currentlySelectedOption == i ? 1 : 0.5f;
            }

            if (m_directionalInputTimer <= 0)
            {

                if (directionalInput.x < -INPUT_THRESHOLD)
                {
                    if (m_currentlySelectedOption == 1 && m_stretchFill == 1)
                    {
                        m_stretchFill = 0;
                        PlayerPrefs.SetInt("stretchFill", m_stretchFill);
                    }
                    else if (m_currentlySelectedOption == 2 && m_pixelPerfect == 1)
                    {
                        m_pixelPerfect = 0;
                        PlayerPrefs.SetInt("pixelPerfect", m_pixelPerfect);

                    }
                }

                if (directionalInput.x > INPUT_THRESHOLD)
                {
                    if (m_currentlySelectedOption == 1 && m_stretchFill == 0)
                    {
                        m_stretchFill = 1;
                        PlayerPrefs.SetInt("stretchFill", m_stretchFill);
                    }
                    else if (m_currentlySelectedOption == 2 && m_pixelPerfect == 0)
                    {
                        m_pixelPerfect = 1;
                        PlayerPrefs.SetInt("pixelPerfect", m_pixelPerfect);

                    }
                }
            }

            if (selectPressed && m_selectedInputTimer <= 0)
            {
                m_selectedInputTimer = INPUT_BUFFER;

                if (m_currentlySelectedOption == 0)
                {
                    UnPause();
                }
                else if (m_currentlySelectedOption == 1)
                {
                    m_stretchFill = m_stretchFill == 0 ? 1 : 0;
                    PlayerPrefs.SetInt("stretchFill", m_stretchFill);
                }
                else if (m_currentlySelectedOption == 2)
                {
                    m_pixelPerfect = m_pixelPerfect == 0 ? 1 : 0;
                    PlayerPrefs.SetInt("pixelPerfect", m_pixelPerfect);

                }
                else if (m_currentlySelectedOption == 3)
                {
                    SceneManager.LoadScene("Demo");
                }
                else if (m_currentlySelectedOption == 4)
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();

#endif
                }
            }
        }

        public void UnPause()
        {
            Time.timeScale = 1;
            canvas.alpha = 0;
        }

        public void Pause()
        {
            canvas.alpha = 1;
            Time.timeScale = 0;
        }

       
    }
}
