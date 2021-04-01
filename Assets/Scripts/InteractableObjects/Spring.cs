using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float springPower = 20;
    
    
    private Animator _animator;
    private static readonly int Spring1 = Animator.StringToHash("spring");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.WasHitfromTopSide())
        {
            var player = other.gameObject.GetComponent<Player>();
            if (!player)
                return;
            player.GetSprung(springPower);
            _animator.SetTrigger(Spring1);
            AudioManager.Instance.SpringAudio.PlaySound();
        }
    }
}
