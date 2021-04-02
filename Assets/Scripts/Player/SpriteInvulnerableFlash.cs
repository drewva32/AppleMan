using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class SpriteInvulnerableFlash : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _regularColor;
    private Color slightlyTransparent = new Color(1f, 1, 1, 0.1f);

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _regularColor = _spriteRenderer.color;
    }

    public void FlashSprite()
    {
        StartCoroutine(FlashSpriteRoutine());
    }

    private IEnumerator FlashSpriteRoutine()
    {
        float timer = 0;
        while (timer < 1.35f)
        {
            timer += Time.deltaTime;
            _spriteRenderer.color = Color.Lerp(_regularColor, slightlyTransparent, math.sin(timer * 30));
            yield return null;
        }
        _spriteRenderer.color = _regularColor;
    }
}
