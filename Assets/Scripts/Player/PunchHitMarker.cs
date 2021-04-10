using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHitMarker : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Hit = Animator.StringToHash("hit");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void HitMarker()
    {
        _animator.SetTrigger(Hit);
    }
}
