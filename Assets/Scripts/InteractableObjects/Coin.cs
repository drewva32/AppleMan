using System;
using UnityEngine;

public class Coin : MonoBehaviour, IHaveAnimationEndEvent
{
    private Animator _animator;
    private static readonly int Pickup = Animator.StringToHash("pickup");
    private bool _isPickedUp;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Reset()
    {
        _isPickedUp = false;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player && !_isPickedUp)
        {
            AppleGameManager.Instance.AddCoin();
            _animator.SetTrigger(Pickup);
            _isPickedUp = true;
        }
    }

    //called by animation event.
    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }

    
}
