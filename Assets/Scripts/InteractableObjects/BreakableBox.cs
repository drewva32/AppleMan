using System;
using System.Collections;
using UnityEngine;

public class BreakableBox : MonoBehaviour , IPlayerInteractions
{
    private Animator _animator;
    private Collider2D _collider;
    private static readonly int Property = Animator.StringToHash("break");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    public void OnEnable()
    {
        _collider.enabled = true;
    }

    public void Break()
    {
        _animator.SetTrigger(Property);
        _collider.enabled = false;
        StartCoroutine(DisableAfterBreaking());
    }

    private IEnumerator DisableAfterBreaking()
    {
        //0.66 is the current length of the box breaking animation
        yield return new WaitForSeconds(0.66f);
        gameObject.SetActive(false);
    }


    public void TakePlayerHit(int damageAmount, Vector3 directionToPlayer, float amountOfForce)
    {
        Break();
    }
}