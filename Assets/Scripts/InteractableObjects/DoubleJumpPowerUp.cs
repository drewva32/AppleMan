using System;
using System.Collections;
using UnityEngine;

public class DoubleJumpPowerUp : MonoBehaviour, IHaveAnimationEndEvent
{
    [SerializeField] private float colorChangeSpeed = 1;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject transitionText;
    
    
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _collected;
    private static readonly int Pickup = Animator.StringToHash("pickup");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.color = Color.HSVToRGB(1, 1, 1);
        if(playerData.amountOfJumps ==2)
            gameObject.SetActive(false);
    }

    private void Start()
    {
        transitionText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player && !_collected)
        {
            if(transitionText != null)
                transitionText.gameObject.SetActive(true);
            _collected = true;
            playerData.amountOfJumps = 2;
            _animator.SetTrigger(Pickup);
            if (AudioManager.Instance != null)
                AudioManager.Instance.OneUpAudio.PlayCollectSound();
        }
    }

    private void Update()
    {
        float h, s, v;
        Color.RGBToHSV(_spriteRenderer.color,out h, out s, out v);

        _spriteRenderer.color = Color.HSVToRGB(h + Time.deltaTime * colorChangeSpeed, s, v);
    }

    
    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }

}
