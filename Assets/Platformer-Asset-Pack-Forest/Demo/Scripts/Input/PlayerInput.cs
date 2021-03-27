using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    [RequireComponent(typeof(Player2D))]
    public class PlayerInput : MonoBehaviour
    {
        private Player2D m_player;
        private Vector2 m_directionalInput;
        private Vector2 m_directionalInputLast;

        private void Start()
        {
            m_player = GetComponent<Player2D>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseMenu.Instance.Pause();
            }

            if (!m_player.updateInput)
                return;

            m_directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            m_player.SetDirectionalInput(m_directionalInput);

            if (m_directionalInput.y > 0 && Mathf.Abs(m_directionalInputLast.y) <= 0)
            {
                m_player.OnJumpInputDown();
            }

            if (Mathf.Abs(m_directionalInput.y) <= 0 && m_directionalInputLast.y > 0)
            {
                m_player.OnJumpInputUp();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                m_player.OnLiftButtonPressed();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                m_player.OnAttackButtonPressed();
            }

            m_directionalInputLast = m_directionalInput;
        }
    }
}
