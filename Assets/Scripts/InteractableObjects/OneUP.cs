using System;
using UnityEngine;


public class OneUP : MonoBehaviour,IHaveAnimationEndEvent
{
    private bool _isPickedUp;
    private Animator _animator;
    private static readonly int Pickup = Animator.StringToHash("pickup");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player && !_isPickedUp)
        {
            AppleGameManager.Instance.ChangeLives(true);
            _animator.SetTrigger(Pickup);
            _isPickedUp = true;
        }
    }
    
    public void Reset()
    {
        _isPickedUp = false;
        gameObject.SetActive(true);
    }
    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
