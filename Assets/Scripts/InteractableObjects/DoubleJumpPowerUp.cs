using System;
using System.Collections;
using UnityEngine;

public class DoubleJumpPowerUp : MonoBehaviour
{
    [SerializeField] private float colorChangeSpeed = 1;
    [SerializeField] private PlayerData playerData;
    
    
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.color = Color.HSVToRGB(1, 1, 1);
        if(playerData.amountOfJumps ==2)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            playerData.amountOfJumps = 2;
            gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        float h, s, v;
        Color.RGBToHSV(_spriteRenderer.color,out h, out s, out v);

        _spriteRenderer.color = Color.HSVToRGB(h + Time.deltaTime * colorChangeSpeed, s, v);
    }
    
    

}
