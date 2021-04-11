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
            if(AppleGameManager.Instance != null)
                AppleGameManager.Instance.AddCoin();
            if(AudioManager.Instance != null)
                AudioManager.Instance.CoinAudio.PlayCollectSound();
            _animator.SetTrigger(Pickup);
            _isPickedUp = true;
        }
    }
    private void OnEnable()
    {
        if(_isPickedUp)
            gameObject.SetActive(false);
    }

    //called by animation event.
    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }

    
}
